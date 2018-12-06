using Group14_BevoBooks.DAL;
using System;
using System.Linq;

namespace Group14_BevoBooks.Utilities
{
    public static class GenerateNextBookNumber
    {
        public static Int32 GetNextBookNumber(AppDbContext db)
        {
            Int32 intMaxBookNumber; //the current maximum course number
            Int32 intNextBookNumber; //the course number for the next class

            if (db.Books.Count() == 0) //there are no courses in the database yet
            {
                intMaxBookNumber = 789300; //course numbers start at 3001
            }
            else
            {
                intMaxBookNumber = db.Books.Max(c => c.BookID); //this is the highest number in the database right now
            }

            //add one to the current max to find the next one
            intNextBookNumber = intMaxBookNumber + 1;

            //return the value
            return intNextBookNumber;
        }

    }
}
