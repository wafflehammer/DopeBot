using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using Discord.Net;
using DSharpPlus.CommandsNext;

class DopeBot
    {
        static DiscordClient discord;
        static string fileUrl = "https://gist.githubusercontent.com/wafflehammer/e40c46c7d0ebe1059e1fc00d3c2cd129/raw/2c9fd11103c8c939583cf6197b69a022f7858d9d/gistfile1.txt";
        private static string tokenFileName = "token.txt";
        static string fileNameToSave = "CrackMeSolutions.txt";
        static string fullPathWhereToSave;
        static int timeoutInMilliSec;
        static void Main(string[] args)
        {
         
            
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }


        static async Task<string> DownloadDocument()
        {  
            fullPathWhereToSave = Path.Combine(  Path.GetTempPath(), fileNameToSave);
            bool success = FileDownloader.DownloadFile(fileUrl, fullPathWhereToSave, timeoutInMilliSec);
       
            await Task.Delay(-1);
            return System.IO.File.ReadAllText(fullPathWhereToSave);

        }

      
        static async Task<string> ReadToken()
        {
            return System.IO.File.ReadAllText(Path.Combine(System.Environment.CurrentDirectory, tokenFileName));
        }
        struct solutiontable
        {
            private byte id;
            private string solution;

            public solutiontable(byte id, string solution)
            {
                this.solution = solution;
                this.id = id;
            }
        }

        static async Task MainAsync(string[] args)
        {

            string readtoken = await ReadToken();

            DiscordConfiguration config = new DiscordConfiguration
            {
                TokenType = TokenType.Bot,
                Token = readtoken
            };
     
            discord = new DiscordClient(config);
            
            string downloadedtable = await DownloadDocument();
           
            
            List <solutiontable> MasterTable = null;
            string[] parsedtable = downloadedtable.Split("-", StringSplitOptions.RemoveEmptyEntries);
            for (byte b = 0; b < parsedtable.Length; b += 2)
            {
                solutiontable entry = new solutiontable(byte.Parse(parsedtable[b]), parsedtable[b + 1]);
                MasterTable.Add(entry);

            }

       

         discord.MessageCreated += async (s, e) =>
                 {
                     if (e.Message.Content.ToLower().StartsWith("..solve"))
                     {  //format is solve-0001-
                         try
                         {
                             string[] query = e.Message.Content.Split('-');
                             int crackID = int.Parse(query[1]);

                             var solution = query[2];
                             await e.Message.RespondAsync(crackID + " " + solution);
                             await e.Message.DeleteAsync();
                         }
                         catch (Exception ex)
                         {
                             await e.Message.RespondAsync("nigga wat?"); 
                             //await e.Message.RespondAsync(ex.ToString());
                         }

                         
                    
                     }
                 };
         
            await discord.ConnectAsync();
            await Task.Delay(-1);
            
            
            
        }
        }
        
        
    