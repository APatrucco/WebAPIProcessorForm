using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebAPIWindowsApp
{
    public partial class TreeWindow : Form
    {
        static readonly HttpClient client = new HttpClient();

        #region Instantiate Lists
        
        public List<Comment> comments = new List<Comment>();
        public List<Post> posts = new List<Post>();
        public List<User> users = new List<User>();
        public List<ToDo> todos = new List<ToDo>();

        #endregion

        public TreeWindow()
        {
            InitializeComponent();
        }

        private async void TreeWindow_Load(object sender, EventArgs e)
        {
            await PopulateTreeViewAsync();
        }

        private async Task PopulateTreeViewAsync()
        {
            TreeNode treeNode1 = TreeView1.TopNode;

            #region Before populating data.

            var timer = new Stopwatch();
            timer.Start();
            searchProgress.Visible = true;

            #endregion

            try
            {
                #region Fetch JSON from API.

                string fetchedComments = await client.GetStringAsync("https://jsonplaceholder.typicode.com/comments");
                string fetchedPosts = await client.GetStringAsync("https://jsonplaceholder.typicode.com/posts");
                string fetchedUsers = await client.GetStringAsync("https://jsonplaceholder.typicode.com/users");
                string fetchedToDos = await client.GetStringAsync("https://jsonplaceholder.typicode.com/todos");

                #endregion

                #region Deserialize JSON into JArray.

                JArray commentArray = JsonConvert.DeserializeObject<dynamic>(fetchedComments);
                JArray postArray = JsonConvert.DeserializeObject<dynamic>(fetchedPosts);
                JArray userArray = JsonConvert.DeserializeObject<dynamic>(fetchedUsers);
                JArray toDoArray = JsonConvert.DeserializeObject<dynamic>(fetchedToDos);

                #endregion

                #region Convert JArrays into List<T>.

                foreach (var post in postArray)
                {
                    Post newPost = new Post();
                    JsonConvert.PopulateObject(post.ToString(), newPost);

                    posts.Add(newPost);
                }

                foreach (var comment in commentArray)
                {
                    Comment newComment = new Comment();
                    JsonConvert.PopulateObject(comment.ToString(), newComment);

                    comments.Add(newComment);
                }

                foreach (var todo in toDoArray)
                {
                    ToDo newToDo = new ToDo();
                    JsonConvert.PopulateObject(todo.ToString(), newToDo);

                    todos.Add(newToDo);
                }

                foreach (var user in userArray)
                {
                    User newUser = new User();
                    JsonConvert.PopulateObject(user.ToString(), newUser);

                    foreach (var todo in todos)
                    {
                        if (todo.UserId == newUser.ID)
                            newUser.ToDos.Add(todo);
                    }

                    foreach (var post in posts)
                    {
                        if (post.UserID == newUser.ID)
                            newUser.Posts.Add(post);
                    }

                    foreach (var comment in comments)
                    {
                        if (comment.PostID == newUser.ID)
                            newUser.Comments.Add(comment);
                    }

                    users.Add(newUser);
                }

                #endregion

                TreeView1.BeginUpdate();
                
                foreach (var user in users)
                {
                    treeNode1.Nodes.Add(user.Name);
                    treeNode1.Nodes[users.IndexOf(user)].Nodes.Add("Username");
                    treeNode1.Nodes[users.IndexOf(user)].Nodes[0].Nodes.Add(user.UserName);
                    treeNode1.Nodes[users.IndexOf(user)].Nodes.Add("Address");
                    treeNode1.Nodes[users.IndexOf(user)].Nodes[1].Nodes.Add($"Street: {user.Address.Street}");
                    treeNode1.Nodes[users.IndexOf(user)].Nodes[1].Nodes.Add($"City: {user.Address.City}");
                    treeNode1.Nodes[users.IndexOf(user)].Nodes[1].Nodes.Add($"ZipCode: {user.Address.ZipCode}");
                    treeNode1.Nodes[users.IndexOf(user)].Nodes.Add("Comments");
                    foreach (var comment in user.Comments)
                    {
                        treeNode1.Nodes[users.IndexOf(user)].Nodes[2].Nodes.Add($"Comment from {comment.Email} --- {comment.Body}");
                    }
                    treeNode1.Nodes[users.IndexOf(user)].Nodes.Add("Posts");
                    foreach (var post in user.Posts)
                    {
                        treeNode1.Nodes[users.IndexOf(user)].Nodes[3].Nodes.Add($"{post.Title} --- {post.Body}");
                    }
                }
                
                TreeView1.EndUpdate();
            }
            #region Exception Handling

            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Exception Caught!\n{ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Caught!\n{ex.Message}");
            }

            #endregion

            #region After populating data.

            searchProgress.Visible = false;

            #endregion
        }

        private async void SearchButton_Click(object sender, EventArgs e)
        {
            searchProgress.Visible = true;

            DataGrid dataGrid = new DataGrid();

            if (!string.IsNullOrWhiteSpace(userIdBox.Text))
            {
                int.TryParse(userIdBox.Text, out int userId);
                var queryId = await Task.Run(() => CustomQuery.Query(users, userId));
                dataGridView1.DataSource = queryId.ToList();
                dataGridView1.Columns["Address"].Visible = false;
                dataGridView1.Columns["Company"].Visible = false;

                statusText.Text = $"{queryId.Count()} users found!";
            }
            if (!string.IsNullOrWhiteSpace(userNameBox.Text))
            {
                var userName = userNameBox.Text;
                statusText.Text = $"Finding users containing '{userName}'";

                var queryName = await Task.Run(() => CustomQuery.Query(users, userName));
                dataGridView1.DataSource = queryName.ToList();
                dataGridView1.Columns["Address"].Visible = false;
                dataGridView1.Columns["Company"].Visible = false;

                statusText.Text = $"{queryName.Count()} users found!";
            }
            dataGridView1.Refresh();

            searchProgress.Visible = false;
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            userIdBox.Text = "";
            userNameBox.Text = "";
        }
    }
}
