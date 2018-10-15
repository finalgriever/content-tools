using System;
using System.IO;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FinalGriever.ContentTools.PrepReplays
{
    public class Program
    {
        static void Main(string[] args)
        {
            try{
                var app = new CommandLineApplication();
                var prefixOption = app.Option("-p|--prefix <value>", "The file prefix", CommandOptionType.SingleValue);
                var sourceOption = app.Option("-s|--source <value>", "The source folder", CommandOptionType.SingleValue);
                var destinationOption = app.Option("-d|--destination <value>", "The destination folder the zip will be saved to", CommandOptionType.SingleValue);
                var configFileOption = app.Option("-c|--config <value>", "The config file", CommandOptionType.SingleValue);

                app.OnExecute(() => {
                    string source, destination, prefix;
                    GetSourceAndDestination(prefixOption, sourceOption, destinationOption, configFileOption, out prefix, out source, out destination);
                    var mover = new FileMover(source, destination, prefix);
                    mover.Move();
                    return 0;
                });

                app.Execute(args);
            }
            catch(UserException e)
            {
                Console.WriteLine(e.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine("Oops! Something went wrong! Tell Jamie about it. Here are the details: " + e.Message);
            }
        }

        public static void GetSourceAndDestination(CommandOption prefixOption, CommandOption sourceOption, CommandOption destinationOption, CommandOption configFileOption, out string prefix, out string source, out string destination)
        {
            var specific = sourceOption.HasValue() && destinationOption.HasValue();
            var useConf = configFileOption.HasValue();
            if(!prefixOption.HasValue()) {
                prefix = "";
            } else {
                prefix = prefixOption.Value();
            }
            if(specific && useConf) {
                throw new UserException("Specify folders or a config file - not both.");
            } else if(!specific && !useConf) {
                throw new UserException("Neither config nor target folders were specified.");
            } else if (specific) {
                source = sourceOption.Value();
                destination = sourceOption.Value();
            } else {
                var configLocation = configFileOption.Value();
                var fullPath = System.IO.Path.GetFullPath(configLocation);
                using (StreamReader file = File.OpenText(fullPath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    Config config = (Config)serializer.Deserialize(file, typeof(Config));
                    source = config.Source;
                    destination = config.Destination;
                    prefix = config.Prefix;
                }
            }
        }
    }

}
