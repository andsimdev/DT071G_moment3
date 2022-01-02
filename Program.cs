/* Moment 3 i kursen DT071G, Programmering i C#.NET av Simon Andersson, baserat på kodexempel av Mikael Hasselmalm */

using System;

// Funktionalitet för bl.a. listor
using System.Collections.Generic;

// Funktionalitet för bl.a. filer
using System.IO;

// Funktionalitet för JSON
using System.Text.Json;

// Huvudnamespace för uppgiften

namespace moment3
{
    // Klass för gästboksinlägg
    public class Post
    {
        // Skapar fält för posterna, access till den privata strängen via den publika
        private string one_post;
        public string One_post
        {
            // Set & get
            set { this.one_post = value; }
            get { return this.one_post; }
        }
    }

    // Klass för att hantera gästboksinlägg
    public class PostStore
    {
        // Variabel för JSON-fil med lagrade inlägg
        private string file = @"posts.json";

        // Skapa lista med inläggen
        private List<Post> posts = new List<Post>();

        // Kontruerare för att hämta all data från JSON-fil
        public PostStore()
        {
            // Kontrollera att JSON-filen finns och läs ut dess data
            if (File.Exists(@"posts.json") == true)
            {
                // Lagra datan som sträng
                string jsonString = File.ReadAllText(file);

                // Deserialisera strängen till JSON-format och lagra i listan
                posts = JsonSerializer.Deserialize<List<Post>>(jsonString);
            }
        }

        // Lagra nytt inlägg
        public Post addPost(Post post)
        {
            // Lägg till nya inlägget till listan
            posts.Add(post);

            // Skriv ut listan till JSON-filen (anropa metoden write)
            write();

            // Returnera posten
            return post;
        }

        // Radera inlägg (returnerar raderat index)
        public int deletePost(int index)
        {
            // Radera posten med medskickat index från listan
            posts.RemoveAt(index);

            // Skriv listan till JSON-filen
            write();

            // Returnera raderat index
            return index;
        }

        // Hämta alla inlägg som lista
        public List<Post> getPosts()
        {
            return posts;
        }

        // Skriv listan till JSON-filen
        private void write()
        {
            // Serialisera alla objekt från listan posts
            var jsonString = JsonSerializer.Serialize(posts);

            // Skriv listan till JSON-filen
            File.WriteAllText(file, jsonString);
        }
    }

    // Huvudprogrammet
    class Program
    {
        static void Main(string[] args)
        {
            // Skapa nytt objekt av klassen PostStore
            PostStore poststore = new PostStore();

            // Iterera ut info och inlägg till skärmen
            int i = 0;
            while(true)
            {
                // Rensa konsolen
                Console.Clear();

                // Inaktivera markören
                Console.CursorVisible = false;

                // Skriv ut information
                Console.Write
                    ("*** SIMONS GÄSTBOK ***\n\n" +
                    "Instruktioner:\n" +
                    "1. Skapa nytt inlägg\n" +
                    "2. Ta bort inlägg\n" +
                    "A. Avsluta\n" +
                    "----------\n\n");

                // Loopa igenom och skriv ut alla inlägg
                i = 0;
                foreach(Post post in poststore.getPosts())
                {
                    Console.WriteLine("(" +i++ +  ") " + post.One_post);
                }

                // Hämta användarens input och kör olika case beroende på inamtning
                int input = (int)Console.ReadKey(true).Key;

                switch(input)
                {
                    // Vid tryck på 1, skapa nytt inlägg
                    case '1':
                        // Visa markören
                        Console.CursorVisible = true;

                        // Anropa metod för att läsa inmatat inlägg
                        readInput();
                       
                        // Metod för att läs in inmatad text
                        void readInput()
                        {
                            // Visa instruktion och läs in inmatat namn
                            Console.WriteLine("\nAnge ditt namn:");
                            string inputname = Console.ReadLine();

                            // Kontrollera att inmatat värde ej är tomt
                            if(String.IsNullOrEmpty(inputname))
                            {
                                // Skriv ut felmeddelande
                                Console.WriteLine("\nInmatat värde får ej vara tomt, försök igen.");

                                // Anropa metoden på nytt
                                readInput();
                            }
                            // Annars, fortsätt och efterfråga inlägg
                            else
                            {
                                // Visa instruktion och läs in inmatat inlägg
                                Console.WriteLine("\nSkriv ditt inlägg nedan:");
                                string inputpost = Console.ReadLine();

                                // Kontrollera att inmatat värde ej är tomt
                                if (String.IsNullOrEmpty(inputpost))
                                {
                                    // Skriv ut felmeddelande och kör om metoden
                                    Console.WriteLine("\nInmatat värde får ej vara tomt, försök igen.");
                                    readInput();
                                }
                                // Annars, lagra inlägget
                                else
                                {
                                    // Skapa nytt objekt av klassen Post och lagra inlägget i objektet
                                    Post obj = new Post();
                                    obj.One_post = inputname + " - " + inputpost;
                                    poststore.addPost(obj);
                                }
                            }
                        }
                        break;

                    // Vid tryck på 2, radera inlägg
                    case '2':
                        // Visa markören
                        Console.CursorVisible = true;

                        // Anropa metod för att läsa inmatat index
                        readIndex();

                        // Metod för att läsa inmatat input
                        void readIndex()
                        {
                            // Visa instruktion och läs in inmatat  index
                            Console.WriteLine("\nAnge index för inlägget att radera: ");
                            string index = Console.ReadLine();

                            // Kontrollera att något index angetts
                            if(String.IsNullOrEmpty(index))
                            {
                                // Skriv ut felmeddelande och kör om metoden
                                Console.WriteLine("\nInmatat värde får ej vara tomt, försök igen.");
                                readIndex();
                            } 
                            // Annars, radera posten för inmatat index
                            else
                            {
                                // Skapa tom variabel för konverterat index
                                int intindex;

                                // Kontrollera att värdet går att konvertera till siffror
                                if(int.TryParse(index, out intindex))
                                {
                                    // Anropa objektets metod för att radera inlägg
                                    poststore.deletePost(intindex);
                                } 
                                // Annars, skriv ut felmeddelande och kör om metoden
                                else
                                {
                                    Console.WriteLine("\nInmatat värde måste vara en siffra, försök igen.");
                                    readIndex();
                                }
                            }
                        }
                        break;

                    // Vid tryck på A, avsluta programmet
                    case 65:
                        // Avsluta applikationen, returnera kod 0 (inget fel)
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}