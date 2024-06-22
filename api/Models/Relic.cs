using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Relic
    {
        [Key]
        public int Id { get; set; }
        // Actual name displayed for the relic
        public string? Name { get; set; } = null;
        // Rank of the relic displayed on wiki: Green, Blue, Purple (A,AA,AAA), Orange (S,SS,SSS)
        public string? Rank { get; set; } = null;
        // The highest affect stat of the relic and thus it's category
        public string? AFFCT { get; set; } = null;
        // Probably useless but it's on the wiki
        public string? Birthday { get; set; } = null;
        public string? ImageUri { get; set; } = "/images/MissingRelic.png";
        // Stats of the relic in map format, 1 star = "", 2 star = "", etc...
        public Dictionary<string, string>? Stats { get; set; } = null;
        // Where the relic comes from and what the odds of getting it are
        // Sources are seperated by comma so I don't need to store it as JSON
        public string? Sources { get; set; } = null;
        // The special effects of the relic listen in the psuedo description of the relic, i.e: Snail ATK +90
        // Usually there is more than 1 special effect on a orange relic so they're seperated by comma delimiter string.Split("effect1,effect2,effect3",",");
        public string? Effect { get; set; } = null;
        // Similar to stats skills are either null or can have 1-3 depending on rank
        // Will be null if green otherwise 1 = "skill info", 2 = "", etc...
        public Dictionary<string,string>? Skills { get; set; } = null;
        // Some relics have a description of the historical importance of it, not all of them do, so nullable :P
        public string? Description { get; set; } = null;
    }
}