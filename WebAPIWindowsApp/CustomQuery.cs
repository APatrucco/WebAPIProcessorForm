using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WebAPIWindowsApp
{
    public class CustomQuery
    {
        public static IEnumerable<dynamic> Query(IEnumerable<dynamic> someList)
        {
            Console.WriteLine("Please enter user ID: ");
            int inputID = int.Parse(Console.ReadLine());

            dynamic query = from dynamic item in someList
                            where item.ID.Equals(inputID)
                            select item;

            return query;
        }

        public static IEnumerable<dynamic> Query(IEnumerable<dynamic> someList, int userId)
        {
            List<dynamic> myList = new List<dynamic>();

            dynamic query = from dynamic item in someList
                            where item.ID.Equals(userId)
                            select item;

            return query;
        }

        public static IEnumerable<dynamic> Query(IEnumerable<dynamic> someList, string userInput)
        {
            dynamic query = from dynamic item in someList
                            where item.Name.ToLower().Contains(userInput.ToLower())
                            select item;

            return query;
        }
    }
}