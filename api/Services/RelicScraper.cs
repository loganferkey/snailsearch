using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class RelicScraper : IWebScraper
    {
        private readonly ApplicationDbContext _context;
        private ConsoleUtil _consoleUtil;
        // Home page of the wiki incase it is necessary later on
        private readonly string wikiUrl = "https://supersnail.wiki.gg/wiki/Super_Snail_Wiki";
        // Main link to the page that holds all of the relics that will be scraped
        private readonly string allRelicUrl = "https://supersnail.wiki.gg/wiki/Relics";
        // This is necessary because supreme relics aren't include inside the all relics url
        private readonly string supremeRelicUrl = "https://supersnail.wiki.gg/wiki/Supreme_Relic";
        public RelicScraper(ApplicationDbContext context, ConsoleUtil consoleUtil)
        {
            this._context = context;
            this._consoleUtil = consoleUtil;
        }

        public void Seed()
        {
            // Performance/debugging
            int newRelic = 0; // Count of new relics added to database
            int updatedRelic = 0; // Count of relics that had differing information and were updated in the database!
            int noChanges = 0;

            // Get all of the relics first, so we can compare real time to webscraped relics to see if we need to update them or add a new relic to the database
            _consoleUtil.StartTimer("Starting relic fetch from db...", ConsoleColor.DarkMagenta, null);

            List<Relic> allRelics = _context.Relics.ToList();
            // I might use this hashtable later for comparing relics while parsing if I don't like how fast normal list lookup is
            Dictionary<string, Relic> relicHash = allRelics.ToDictionary(r => r.Name);

            _consoleUtil.StopTimer("Database relic fetch => [t]ms");

            // Start parsing the relic page for all relics and compare them to existing relics
            _consoleUtil.StartTimer("Starting webscrape for normal relics...", ConsoleColor.DarkMagenta, null);

            // Start webscrape by loading the html from the url and begin parsing with xpath
            HtmlWeb hap = new HtmlWeb(); // utility class from htmlagilitypack
            HtmlDocument? doc = hap.Load(this.allRelicUrl);
            if (doc == null) 
            {
                Console.WriteLine("Error requesting general relic page, exiting...");
                return;
            }
            // Using xpath to grab all relics on the page to be parsed for new/updated information
            HtmlNodeCollection? scrapedRelics = doc.DocumentNode.SelectNodes(@"//*[@id=""mw-content-text""]/div[1]/table[3]/tbody/tr");
            if (scrapedRelics == null)
            {
                // Error selecting nodes via xpath or query failed on the document
                Console.WriteLine("Error selecting nodes on document, exiting...");
                return;
            };

            _consoleUtil.StopTimer($"Relics => ({newRelic}) new, ({updatedRelic}) updated, ({noChanges}) no changes: [t]ms parse");
        }
        public void SeedDetail()
        {
        }
    }
}