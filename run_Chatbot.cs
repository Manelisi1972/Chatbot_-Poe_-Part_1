 using System.Collections;
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
        
        
        private Dictionary<string, List<string>> keywordResponses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        private string userName = "";
        private string lastTopic = "";
        private string favoriteTopic = "";
        private string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logss");
        private string logFilePath = "";

        private List<string> positiveEncouragements = new List<string>
        {
            "Don't worry, cybersecurity can seem tough at first, but I'm here to help!",
            "It's great that you're curious — asking questions is how you get stronger online.",
            "Even when it's frustrating, learning this stuff helps keep you and your info safe!"
        };



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
                 string userName = Console.ReadLine()?.Trim();


                if (string.IsNullOrEmpty(userName))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: You must enter a name before proceeding. ");
                    Console.ResetColor();
                    continue;

                }//end if

                // load and display user history if it exists
                string logFilePath = $"ChatHistory_{userName}.txt";

                

                if (File.Exists(logFilePath))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Welcome back, {userName}! Here's your previous conversation:\n");
                    Console.ResetColor();
                    
                    string history = File.ReadAllText(logFilePath);
                    Console.WriteLine(history);

                }//end if statement
                else
                {
                    File.WriteAllText(logFilePath, $"Conservation history for {userName} - Started on {DateTime.Now}\n\n");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"\n Hi {userName}, lets start a new cybersecurity chat !");
                    Console.ResetColor();
                }//end of else 

                
                Console.WriteLine(" [WishiChatBot]:  Would you like to ask a question? (yes/no)");
                string choice = Console.ReadLine().Trim().ToLower();

                switch (choice)
                {
                    case "yes":
                        // Now call the method to process the question
                        ProcessUserQuestion();//now includes username in dialogand logs chat
                        break;
                    case "no":
                        Console.WriteLine($"Goodbye! Stay safe online.{userName}");
                        return; // Ends the chatbot completely
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid option. Please type 'yes' or 'no'.");
                        Console.ResetColor();
                        break;
                }//end of switch case 
            }//end of while loop



        }// end of chatbot menu method


        //method to process user question
        private void ProcessUserQuestion()
        {

            //
            store_replies();// Ensure replies are stored

            string logDirectory = "logss";
            Directory.CreateDirectory(logDirectory);
            string logFilePath = Path.Combine(logDirectory, $"{userName}.txt");
            

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nYou can ask about: Purpose, Password Security, Phishing Scams, Privacy ");
            Console.ResetColor();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n{userName} Enter your question (or type 'exit' to quit):");
                string userInput = Console.ReadLine()?.Trim().ToLower();
                Console.ResetColor();

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    string warning = "[WishiChatbot]: Please enter a valid question.";
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(warning);
                    Console.ResetColor();
                    File.AppendAllText(logFilePath, $"User:  {userInput}\nBot: {warning}\n\n");
                    continue;
                }

                if (userInput == "exit")
                {
                    string goodbye = $"Goodbye! Remember to stay safe online, {userName}!";
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(goodbye);
                    Console.ResetColor();
                    Thread.Sleep(1000);
                    Environment.Exit(0);
                }

                // Handle follow-up questions
                if (userInput.Contains("tell me more") || userInput.Contains("more info") || userInput.Contains("explain") || userInput.Contains("what do you mean"))
                {
                    if (!string.IsNullOrEmpty(lastTopic) && keywordResponses.ContainsKey(lastTopic))
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"[WishiChatBot]: Here's a bit more on {lastTopic}: {GetRandomResponse(keywordResponses[lastTopic])}");
                        Console.ResetColor();
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("[WishiChatBot]: Could you tell me which topic you're asking more about?");
                        continue;
                    }//end else
                } //en if

                DetectSentiment(userInput);



                bool found = false;

                foreach (var pair in keywordResponses)
                {
                    if (userInput.Contains(pair.Key))
                    {
                        lastTopic = pair.Key; // Save topic for follow-ups
                        if (string.IsNullOrEmpty(favoriteTopic))
                        {
                            favoriteTopic = pair.Key; // First topic becomes favorite
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"[WishiChatBot]: Great! I'll remember that you're interested in {favoriteTopic}.");
                            Console.ResetColor();
                        }//end if 

                        string response = GetRandomResponse(pair.Value);
                        // sentiment detection
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"[WishiChatBot]: {response}");
                        
                        Console.ResetColor();

                        File.AppendAllText(logFilePath, $"User: {userInput}\nBot: {response}\n\n");
                        found = true;
                        break;
                    }//end if 
                }// end for loop

                if (!found)
                {
                    if (userInput.Contains("more") || userInput.Contains("explain") || userInput.Contains("detail"))
                    {
                        if (!string.IsNullOrEmpty(lastTopic) && keywordResponses.ContainsKey(lastTopic))
                        {
                            string extra = GetRandomResponse(keywordResponses[lastTopic]);
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine($"[WishiChatBot]: Sure! Here's a bit more: {extra}");
                            Console.ResetColor();
                            continue;
                        }//end if 
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            string fallback = "[WishiChatBot]: I'm not sure what you're referring to. Could you specify the topic?";
                            Console.WriteLine(fallback);
                            File.AppendAllText(logFilePath, $"User: {userInput}\nBot: {fallback}\n\n");
                            Console.ResetColor();
                            continue;
                        }// end of else 
                    }//end of if 


                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[WishiChatBot]: Sorry, I couldn't find an answer for that.");
                    Console.WriteLine("Try asking about: Purpose, Password Security, Phishing Scams, or Privacy.");
                    
                    Console.ResetColor();

                }// end if 

                if (!string.IsNullOrEmpty(favoriteTopic) && userInput.Contains("tip") || userInput.Contains("advice"))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[WishiChatBot]: As someone interested in {favoriteTopic}, here's a tip: {GetRandomResponse(keywordResponses[favoriteTopic])}");
                    Console.ResetColor();
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nYou can ask another question or type 'exit' to leave.");
                Console.ResetColor();
            }//end of while 
        }//end of method

        //sentiment detection
        private void DetectSentiment(string userInput)
        {

            if (userInput.Contains("worried") || userInput.Contains("scared"))
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine($"[WishiChatBot]: {positiveEncouragements[0]}");
                Console.ResetColor();
            }
            else if (userInput.Contains("curious") || userInput.Contains("interested"))
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"[WishiChatBot]: {positiveEncouragements[1]}");
                Console.ResetColor();
            }
            else if (userInput.Contains("frustrated") || userInput.Contains("confused"))
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"[WishiChatBot]: {positiveEncouragements[2]}");
                Console.ResetColor();
            }
        }

        private string GetRandomResponse(List<string> responses)
        {
            Random rand = new Random();
            int index = rand.Next(responses.Count);
            return responses[index];
        }//end of method 

        

        //Method for storing replies 

        private void store_replies()
        {
            if (keywordResponses.Count == 0)
            {
                keywordResponses.Add("purpose", new List<string>
        {
            "[ I'm here to help raise awareness about cybersecurity in a fun and friendly way.",
            "[  My purpose is to educate you on staying safe online and avoiding digital threats.",
            " Think of me as your cybersecurity buddy, helping you understand key online safety topics."
        });

                keywordResponses.Add("password", new List<string>
        {
            " Always use a mix of uppercase, lowercase, numbers, and symbols in your passwords.",
            " Avoid using the same password across multiple accounts.",
            "  Consider using a password manager to generate and store strong passwords."
        });

                keywordResponses.Add("phishing", new List<string>
        {
            " Phishing scams often come via emails pretending to be from legitimate sources—double-check URLs before clicking!",
            " Never share personal info through links in unexpected messages.",
            " When in doubt, contact the organization directly instead of replying to suspicious emails."
        });
                keywordResponses.Add("privacy", new List<string>
        {
            "Check app permissions regularly and limit what data you share.",
            "Avoid oversharing personal info on social media—it can be used for identity theft.",
            "Use encrypted messaging apps and review privacy settings on your accounts often."
        });


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

