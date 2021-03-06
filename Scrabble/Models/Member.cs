//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Scrabble.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Member
    {
        public int MemberID { get; set; }
        public string Name { get; set; }
        public string DOB { get; set; }
        public string StreetAddress { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public string DateJoined { get; set; }
        public Nullable<int> NumberOfWins { get; set; }
        public Nullable<int> NumberOfLosses { get; set; }
        public Nullable<int> AverageScore { get; set; }
        public Nullable<int> HighestScore { get; set; }
        public Nullable<System.DateTime> HighestScoreDate { get; set; }
        public string HighestScoreLocation { get; set; }
        public string HighestScoreAgianst { get; set; }
    }
}
