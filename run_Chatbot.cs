﻿using System.Collections;
using System.IO;
using System.Media;
using System.Threading;
using System;
using System.Collections.Generic;




namespace Chatbot__Poe__Part_1
{
    public class run_Chatbot
    {

       
                    // variable declaraton 
        private ArrayList replies = new ArrayList();
        private ArrayList keywords = new ArrayList(); // store keywords separately
        private string userName = "";


        //Constructor
        public run_Chatbot()
        {

            //call methods in the constructor
            Console.Title = "Wishi Cybersecurity Awareness bot";

            AnimateAsciiLogo();
            PlayWelcomeMessage();
            ChatbotMenu();
            
        }// end of constructor

        //chatbot menu method
        private void ChatbotMenu()
        {
            


            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Please enter your name: ");
                userName = Console.ReadLine()?.Trim();


                if (string.IsNullOrEmpty(userName))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: You must enter a name before proceeding. ");
                    Console.ResetColor();
                    continue;

                }//end if 

                Console.WriteLine($"\nWelcome, {userName}! Lets talk about cybersecurity today");
                Console.WriteLine("Would you like to ask a question? (yes/no)");
                string choice = Console.ReadLine().Trim().ToLower();

                switch (choice)
                {
                    case "yes":
                        // Now call the method to process the question
                        ProcessUserQuestion();
                        break;
                    case "no":
                        Console.WriteLine("Goodbye! Stay safe online.");
                        return; // Ends the chatbot completely
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid option. Please type 'yes' or 'no'.");
                        Console.ResetColor();
                        break;
                }
            }//end of while loop



        }// end of chatbot menu method


        //method to process user question
        private void ProcessUserQuestion()
        {

            //
            store_replies();// Ensure replies are stored

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nYou can ask about: Purpose, Password Security, Phishing Scams, Safe Browsing Practices ");
            Console.ResetColor();

            while (true)
            {

                string userInput;

                do
                {

                    
                    // prompt user for question
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Enter your question (or type 'exit' to quit):");
                   
                    userInput = Console.ReadLine()?.Trim().ToLower();

                    if (string.IsNullOrWhiteSpace(userInput))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error: You must enter a valid question.");
                        Console.ResetColor();
                    }//end of if statement

                } while (string.IsNullOrWhiteSpace(userInput)); // Keep asking until a valid question is entered






                //check if user wants to exit
                if (userInput == "exit")
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("Goodbye! remember to stay safe online   "  + userName , "!" );
                        Console.ResetColor();
                        Thread.Sleep(1000); // Pause before exiting
                        Environment.Exit(0); // Completely exit the chatbot
                    }//end of if statement

                // Split the user question into words for better matching
                string[] words = userInput.Split(' ');
                HashSet<int> matchedIndexes = new HashSet<int>();

               


                    foreach (string reply in replies)
                    {
                    for (int i = 0; i < keywords.Count; i++)
                    {
                        if (reply.Contains(keywords[i].ToString()) && !matchedIndexes.Contains(i))
                        {
                            matchedIndexes.Add(i);
                        }//end if
                    }// end of nested loop


                }// end of for loop

                    // Display an error message if no answer found

                    if (matchedIndexes.Count > 0)
                    {
                   
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("[WishiChatBot]: Here’s what I found based on your question: ");
                    foreach (int index in matchedIndexes)
                    {
                        Console.WriteLine($"- {replies[index]}");
                    }//end of for loop 
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nYou can ask another question or type 'exit' to leave.");
                    Console.ResetColor();
                }//end if

                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[WishiChatBot]: Sorry, I couldn't find an answer for that.");
                    Console.WriteLine("Try asking about: Purpose, Password Security, Phishing Scams, or Safe Browsing Practices.");
                    Console.ResetColor();
                    }// end of else 

                }//end of while
        }
        // end of method

        //Method for storing replies 

        private void store_replies()
        {
            if (replies.Count == 0)
            {
                keywords.Add("purpose");
                keywords.Add("password");
                keywords.Add("phishing");
                keywords.Add("browsing");

                // Ensures replies are only stored once
                Console.ForegroundColor = ConsoleColor.Cyan;
                replies.Add("  purpose:  My purpose is to help you with cybersecurity awareness");
                replies.Add("  password security: Use strong passwords with uppercase, lowercase, numbers , and symbols");
                replies.Add("  phishing scams:  Be cautious of emails asking for sensitive information. Verify links before clicking");
                replies.Add(" safe browsing: Always check fort 'HTTPS' in website URLS. Keep your software updated.");


            }// end of if statement
        }// end of replies


        

        //method for the logo
        private void AnimateAsciiLogo()
        {
            string[] logoFames = { @"
██     ██ ██ ███████ ██ ██     
██     ██ ██ ██      ██ ██     
██  █  ██ ██ █████   ██ ██     
██ ███ ██ ██ ██      ██ ██     
 ███ ███  ██ ██      ██ ███████
--------------------------------
   WISHI CYBERSECURITY Awareness BOT [🔐]
--------------------------------
 Stay Safe Online! Think Before You Click!
 
" }; ConsoleColor[] colors = { ConsoleColor.Yellow, ConsoleColor.Green, ConsoleColor.Red, ConsoleColor.Blue };

            for (int i = 0; i < 5; i++)
            {  // Animate logo 5 times

                Console.Clear();
                Console.ForegroundColor = colors[i % colors.Length];

                Console.WriteLine(logoFames[0]);
                Thread.Sleep(500); // Pause for animation effect

            }//end of for loop

            Console.ResetColor();
        }//end of AnimateAsciilogo method

        //method to play welcome message

        private void PlayWelcomeMessage()
        {// Getting full location of the project
            string full_location = System.AppDomain.CurrentDomain.BaseDirectory;

            // Replace the bin\Debug folder in the full_location
            string new_path = full_location.Replace("bin\\Debug\\", "");
            string full_path = Path.Combine(new_path, "welcome.wav");

            // Try and catch
            try
            {

                // Now we create an instance for the SoundPlay class
                using (SoundPlayer play = new SoundPlayer(full_path))
                {
                    // Play the file
                    play.PlaySync();
                } // End of using
            }//end of try catch
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[WishiChatBot]: Error playing welcome message: {ex.Message}");
            } // End of catch
        } // End of welcome message method

        


    }// end of class
}// end of namespace

