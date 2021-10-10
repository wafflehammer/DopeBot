using System;
using System.Threading.Tasks;
using DSharpPlus;
using Discord.Net;
using DSharpPlus.CommandsNext;

class DopeBot
    {
        static DiscordClient discord;

        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = "NzAxMzAwODY0NTI2MzE5NjQ2.XpvfUw.Ho8Ma_aSmXq_qZ_XxSuN7VmSsGI",
                TokenType = TokenType.Bot
            });

         discord.MessageCreated += async (s, e) =>
                 {
                     if (e.Message.Content.ToLower().StartsWith("solve"))
                     {  //format is solve-0001-
                         int crackmeID;
                         string solution;
                         try
                         {
                             string[] query = e.Message.Content.Split('-');
                             crackmeID = int.Parse(query[1]);

                             solution = query[2];
                             await e.Message.RespondAsync(crackmeID + " " + solution);
                             await e.Message.DeleteAsync();
                         }
                         catch (Exception ex)
                         {
                             await e.Message.RespondAsync("sql injecting the bot is not a valid crackme solution (actually, it totally is) so here is how and why you broke me: "); 
                             await e.Message.RespondAsync(ex.ToString());
                         }

                         
                    
                     }
                 };
         
            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
        }
        
        
    