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
        public string Name { get; set; } = string.Empty;
        // Rank of the relic displayed on wiki: Green, Blue, Purple (A,AA,AAA), Orange (S,SS,SSS)
        public string Rank { get; set; } = string.Empty;
        // The highest affect stat of the relic and thus it's category
        public string AFFCT { get; set; } = string.Empty;
        // Where the relic comes from and what the odds of getting it are
        public string Source { get; set; } = string.Empty;
        // All the below stats represent the value of each affect stat on the relic
        public int FAME { get; set; } = 0;
        public int ART { get; set; } = 0;
        public int FTH { get; set; } = 0;
        public int CIV { get; set; } = 0;
        public int TECH { get; set; } = 0;
        // The special effects of the relic listen in the psuedo description of the relic, i.e: Snail ATK +90
        // Usually there is more than 1 special effect on a orange relic so they're seperated by comma delimiter string.Split("effect1,effect2,effect3",",");
        public string Special { get; set; } = string.Empty;
        // Below are up to a maxium of 3 skills per relic (hence nullable), blues have 1, purples have 2, oranges have all 3
        // Allowing nullable just makes it easier
        // Skills are saved in the format of type of skill then a semicolon and skill effect => "fungusfarm;Place in Fungus Farm: Fungus Output +30%"
        // I did this so I can make an enum and easily attach images to skills
        public string? Skill1 { get; set; }
        public string? Skill2 { get; set; }
        public string? Skill3 { get; set; }
        // Some relics have a description of the historical importance of it, not all of them do, so nullable :P
        public string? Description { get; set; }
    }
}