using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Group14_BevoBooks.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Group14_BevoBooks.Models;

namespace Group14_BevoBooks.Seeding

    {
        public static class SeedGenres
        {
            public static void SeedAllGenres(AppDbContext db)
            {
                //check to see if all the languages have already been added
                if (db.Genres.Count() == 13)
                {
                    //exit the program - we don't need to do any of this
                    NotSupportedException ex = new NotSupportedException("Genre count is already 13!");
                    throw ex;
                }
                Int32 intGenresAdded = 0;
                try
                {
                    //Create a list of languages
                    List<Genre> Genres= new List<Genre>();

                    Genre G1 = new Genre { GenreName = "Contemporary Fiction" };
                    Genres.Add(G1);

                    Genre G2 = new Genre { GenreName = "Science Fiction" };
                    Genres.Add(G2);

                    Genre G3 = new Genre { GenreName = "Mystery" };
                    Genres.Add(G3);

                    Genre G4 = new Genre { GenreName = "Suspense" };
                    Genres.Add(G4);

                    Genre G5 = new Genre { GenreName = "Romance" };
                    Genres.Add(G5);

                    Genre G6 = new Genre { GenreName = "Thriller" };
                    Genres.Add(G6);

                    Genre G7 = new Genre { GenreName = "Fantasy" };
                    Genres.Add(G7);

                    Genre G8 = new Genre { GenreName = "Historical Fiction" };
                    Genres.Add(G8);

                    Genre G9 = new Genre { GenreName = "Humor" };
                    Genres.Add(G9);

                    Genre G10 = new Genre { GenreName = "Adventure" };
                    Genres.Add(G10);

                    Genre G11 = new Genre { GenreName = "Horror" };
                    Genres.Add(G11);

                    Genre G12 = new Genre { GenreName = "Poetry" };
                    Genres.Add(G12);

                    Genre G13 = new Genre { GenreName = "Shakespeare" };
                    Genres.Add(G13);


                Genre G;

                    //loop through the list and see which (if any) need to be added
                    foreach (Genre gen in Genres)
                    {
                        //see if the language already exists in the database
                        G = db.Genres.FirstOrDefault(x => x.GenreName == gen.GenreName);

                        //language was not found
                        if (G == null)
                        {
                            //Add the language
                            db.Genres.Add(gen);
                            db.SaveChanges();
                            intGenresAdded += 1;
                        }

                    }
                }
                catch
                {
                    String msg = "Genres Added: " + intGenresAdded.ToString();
                    throw new InvalidOperationException(msg);
                }

            }
        }
    }

