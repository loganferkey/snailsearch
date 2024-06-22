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
        private readonly string allRelicUrl = "https://supersnail.wiki.gg/wiki/Relics";
        // TODO: Add supreme relics as they're not included in base relics page
        private readonly string supremeRelicUrl = "https://supersnail.wiki.gg/wiki/Supreme_Relic";
        private readonly string baseWikiLink = "https://supersnail.wiki.gg";
        private readonly string relicImageDir = "images/relics"; 
        private readonly string sourcesDir = "images/sources"; 
        private readonly string relicSkillDir = "images/relics/skills"; 
        private readonly string baseRelicImgName = "MissingRelic.png"; 
        private readonly string baseRelicSkillImgName = "MissingSkill.png"; 
        private static readonly HttpClient _httpClient = new HttpClient();

        public RelicScraper(ApplicationDbContext context, ConsoleUtil consoleUtil)
        {
            this._context = context;
            this._consoleUtil = consoleUtil;
        }

        public async void Seed()
        {
            // Performance/debugging
            int newRelic = 0; // Count of new relics added to database
            int updatedRelic = 0; // Count of relics that had differing information and were updated in the database!
            int noChanges = 0;

            _consoleUtil.StartTimer("Starting relic fetch from db...", ConsoleColor.DarkMagenta, null);
            List<Relic> dbRelics = _context.Relics.ToList(); // Tracked by EF
            // I might use this hashtable later for comparing relics while parsing if I don't like how fast normal list lookup is
            Dictionary<string, Relic> relicHash = dbRelics.ToDictionary(r => r.Name); // Non tracked list
            _consoleUtil.StopTimer("Database relic fetch => [t]ms");

            // Start parsing the relic page for all relics and compare them to existing relics
            _consoleUtil.StartTimer("Starting webscrape for normal relics...", ConsoleColor.DarkMagenta, null);

            // Start webscrape by loading the html from the url and begin parsing with xpath
            HtmlDocument doc = new HtmlDocument();
            string html = await _httpClient.GetStringAsync(this.allRelicUrl);
            doc.LoadHtml(html);
            if (doc == null) 
            {
                _consoleUtil.WriteColor("Error requesting general relic page, exiting...", ConsoleColor.Red);
                return;
            }
            // Using xpath to grab all relics on the page to be parsed for new/updated information
            HtmlNodeCollection? wikiRelicNodes = doc.DocumentNode.SelectNodes(@"//*[@id=""mw-content-text""]/div[1]/table[3]/tbody/tr");
            if (wikiRelicNodes == null)
            {
                _consoleUtil.WriteColor("Error selecting nodes on document, exiting...", ConsoleColor.Red);
                return;
            };

            // Find the wiki link on every node in the list
            List<string> relicWikiLinks = wikiRelicNodes
                .Select(relicNode => relicNode.SelectSingleNode("./td[1]/a[2]")?.GetAttributeValue("href", ""))
                .Where(wikiLink => !string.IsNullOrEmpty(wikiLink))
                .Select(wikiLink => baseWikiLink + wikiLink)
                .ToList();

            // Create tasks to run in parallel for scraping each wiki page
            IEnumerable<Task<Relic?>> fetchWikiRelics = relicWikiLinks.Select(async link => 
            {
                try 
                {
                    string relicHtml = await _httpClient.GetStringAsync(link);
                    HtmlDocument relicDoc = new HtmlDocument();
                    relicDoc.LoadHtml(relicHtml);

                    // Find all nodes for properties on relic and parse them so format is correct
                    HtmlNode? baseInfoNode = relicDoc.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/div[2]/main/div[3]/div[5]/div[1]/aside"); // Holds name, picture, rank, AFFCT, birthday
                    HtmlNode? statNodes = relicDoc.DocumentNode.SelectSingleNode(@"//*[@id=""mw-content-text""]/div[1]/table[2]/tbody"); // Table containing all stat stars
                    HtmlNode? skillHeaderNode = relicDoc.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/div[2]/main/div[3]/div[5]/div[1]/h2[3]/span"); // Skill tag above skills
                    HtmlNode? descriptionHeaderNode = relicDoc.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/div[2]/main/div[3]/div[5]/div[1]/h2[6]/span"); // Description tag above description
                    HtmlNode? sourceHeaderNode = relicDoc.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/div[2]/main/div[3]/div[5]/div[1]/h2[1]/span"); // Source tag above all sources

                    _consoleUtil.WriteColor($"Parsed {link}", ConsoleColor.Cyan);
                    return new Relic
                    {
                    };
                }
                catch (Exception ex)
                {
                    _consoleUtil.WriteColor($"Failed on link: {link} with error: {ex.Message}", ConsoleColor.Red);
                    return null;
                }
            });

            // Await task completion and compare with db relics/save changes to db
            List<Relic?> relics = (await Task.WhenAll(fetchWikiRelics)).Where(r => r != null).ToList();

            // Loop over returned list of relic objects from wiki and compare them with hashmap of db relics, update relics with same name with non matching properties and update db

            // Who doesn't like colored console output :D
            _consoleUtil.StopTimer($"Relics => ({newRelic}) new, ({updatedRelic}) updated, ({noChanges}) no changes: [t]ms parse");
            _consoleUtil.WriteColor($"Total Relics: {relicWikiLinks.Count}", ConsoleColor.Magenta);
        }

        private void DownloadImageAsync()
        {
        }
    }
}