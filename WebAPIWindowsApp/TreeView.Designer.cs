using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebAPIWindowsApp
{
    partial class treeWindow
    {
        static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private async Task InitializeComponentAsync()
        {

            // 
            // treeView1
            // 
            TreeNode treeNode3 = new TreeNode("Users");

            this.treeView1 = new TreeView();
            this.SuspendLayout();

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

                #region Instantiate Lists.

                List<Comment> comments = new List<Comment>();
                List<Post> posts = new List<Post>();
                List<User> users = new List<User>();
                List<ToDo> todos = new List<ToDo>();

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

                foreach (var user in users)
                {
                    treeView1.BeginUpdate();
                    treeNode3.Nodes.Add(user.Name);
                    treeNode3.Nodes[users.IndexOf(user)].Nodes.Add("Username");
                    treeNode3.Nodes[users.IndexOf(user)].Nodes[0].Nodes.Add(user.UserName);
                    treeNode3.Nodes[users.IndexOf(user)].Nodes.Add("Address");
                    treeNode3.Nodes[users.IndexOf(user)].Nodes[1].Nodes.Add($"Street: {user.Address.Street}");
                    treeNode3.Nodes[users.IndexOf(user)].Nodes[1].Nodes.Add($"City: {user.Address.City}");
                    treeNode3.Nodes[users.IndexOf(user)].Nodes[1].Nodes.Add($"ZipCode: {user.Address.ZipCode}");
                    treeNode3.Nodes[users.IndexOf(user)].Nodes.Add("Comments");
                    foreach (var comment in user.Comments)
                    {
                        treeNode3.Nodes[users.IndexOf(user)].Nodes[2].Nodes.Add($"Comment from {comment.Email} --- {comment.Body}");
                    }
                    treeNode3.Nodes[users.IndexOf(user)].Nodes.Add("Posts");
                    foreach (var post in user.Posts)
                    {
                        treeNode3.Nodes[users.IndexOf(user)].Nodes[3].Nodes.Add($"{post.Title} --- {post.Body}");
                    }
                    treeView1.EndUpdate();
                }
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

            this.treeView1.Location = new Point(12, 12);
            this.treeView1.Name = "treeView1";
            this.treeView1.Nodes.AddRange(new TreeNode[] { treeNode3 });
            this.treeView1.Size = new Size(776, 426);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // treeWindow
            // 
            this.AutoScaleDimensions = new SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 450);
            this.Controls.Add(this.treeView1);
            this.Name = "treeWindow";
            this.Text = "User Tree";
            this.ResumeLayout(false);
        }

        #endregion

        private TreeView treeView1;
    }
}

