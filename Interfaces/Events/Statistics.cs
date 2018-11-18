﻿using DW.ELA.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DW.ELA.Interfaces.Events
{
    public class Statistics: LogEvent
    {
        [JsonProperty("Bank_Account")]
        public Dictionary<string, long> BankAccount { get; set; }

        [JsonProperty("Combat")]
        public Combat Combat { get; set; }

        [JsonProperty("Crime")]
        public Crime Crime { get; set; }

        [JsonProperty("Smuggling")]
        public Smuggling Smuggling { get; set; }

        [JsonProperty("Trading")]
        public Trading Trading { get; set; }

        [JsonProperty("Mining")]
        public Mining Mining { get; set; }

        [JsonProperty("Exploration")]
        public Exploration Exploration { get; set; }

        [JsonProperty("Passengers")]
        public Passengers Passengers { get; set; }

        [JsonProperty("Search_And_Rescue")]
        public SearchAndRescue SearchAndRescue { get; set; }

        [JsonProperty("TG_ENCOUNTERS")]
        public TgEncounters TgEncounters { get; set; }

        [JsonProperty("Crafting")]
        public Crafting Crafting { get; set; }

        [JsonProperty("Crew")]
        public Crew Crew { get; set; }

        [JsonProperty("Multicrew")]
        public Multicrew Multicrew { get; set; }

        [JsonProperty("Material_Trader_Stats")]
        public MaterialTraderStats MaterialTraderStats { get; set; }

        [JsonProperty("CQC")]
        public Dictionary<string, long> Cqc { get; set; }
    }

    public class Combat
    {
        [JsonProperty("Bounties_Claimed")]
        public long BountiesClaimed { get; set; }

        [JsonProperty("Bounty_Hunting_Profit")]
        public long BountyHuntingProfit { get; set; }

        [JsonProperty("Combat_Bonds")]
        public long CombatBonds { get; set; }

        [JsonProperty("Combat_Bond_Profits")]
        public long CombatBondProfits { get; set; }

        [JsonProperty("Assassinations")]
        public long Assassinations { get; set; }

        [JsonProperty("Assassination_Profits")]
        public long AssassinationProfits { get; set; }

        [JsonProperty("Highest_Single_Reward")]
        public long HighestSingleReward { get; set; }

        [JsonProperty("Skimmers_Killed")]
        public long SkimmersKilled { get; set; }
    }

    public class Crafting
    {
        [JsonProperty("Count_Of_Used_Engineers")]
        public long CountOfUsedEngineers { get; set; }

        [JsonProperty("Recipes_Generated")]
        public long RecipesGenerated { get; set; }

        [JsonProperty("Recipes_Generated_Rank_1")]
        public long RecipesGeneratedRank1 { get; set; }

        [JsonProperty("Recipes_Generated_Rank_2")]
        public long RecipesGeneratedRank2 { get; set; }

        [JsonProperty("Recipes_Generated_Rank_3")]
        public long RecipesGeneratedRank3 { get; set; }

        [JsonProperty("Recipes_Generated_Rank_4")]
        public long RecipesGeneratedRank4 { get; set; }

        [JsonProperty("Recipes_Generated_Rank_5")]
        public long RecipesGeneratedRank5 { get; set; }
    }

    public class Crew
    {
        [JsonProperty("NpcCrew_TotalWages")]
        public long NpcCrewTotalWages { get; set; }

        [JsonProperty("NpcCrew_Hired")]
        public long NpcCrewHired { get; set; }

        [JsonProperty("NpcCrew_Fired")]
        public long NpcCrewFired { get; set; }

        [JsonProperty("NpcCrew_Died")]
        public long? NpcCrewDied { get; set; }
    }

    public class Crime
    {
        [JsonProperty("Notoriety")]
        public long Notoriety { get; set; }

        [JsonProperty("Fines")]
        public long Fines { get; set; }

        [JsonProperty("Total_Fines")]
        public long TotalFines { get; set; }

        [JsonProperty("Bounties_Received")]
        public long BountiesReceived { get; set; }

        [JsonProperty("Total_Bounties")]
        public long TotalBounties { get; set; }

        [JsonProperty("Highest_Bounty")]
        public long HighestBounty { get; set; }
    }

    public class Exploration
    {
        [JsonProperty("Systems_Visited")]
        public long SystemsVisited { get; set; }

        [JsonProperty("Exploration_Profits")]
        public long ExplorationProfits { get; set; }

        [JsonProperty("Planets_Scanned_To_Level_2")]
        public long PlanetsScannedToLevel2 { get; set; }

        [JsonProperty("Planets_Scanned_To_Level_3")]
        public long PlanetsScannedToLevel3 { get; set; }

        [JsonProperty("Efficient_Scans")]
        public long? EfficientScans { get; set; }

        [JsonProperty("Highest_Payout")]
        public long HighestPayout { get; set; }

        [JsonProperty("Total_Hyperspace_Distance")]
        public long TotalHyperspaceDistance { get; set; }

        [JsonProperty("Total_Hyperspace_Jumps")]
        public long TotalHyperspaceJumps { get; set; }

        [JsonProperty("Greatest_Distance_From_Start")]
        public double GreatestDistanceFromStart { get; set; }

        [JsonProperty("Time_Played")]
        public long TimePlayed { get; set; }
    }

    public class MaterialTraderStats
    {
        [JsonProperty("Trades_Completed")]
        public long? TradesCompleted { get; set; }

        [JsonProperty("Materials_Traded")]
        public long? MaterialsTraded { get; set; }

        [JsonProperty("Encoded_Materials_Traded")]
        public long? EncodedMaterialsTraded { get; set; }

        [JsonProperty("Raw_Materials_Traded")]
        public long? RawMaterialsTraded { get; set; }

        [JsonProperty("Grade_1_Materials_Traded")]
        public long? Grade1_MaterialsTraded { get; set; }

        [JsonProperty("Grade_2_Materials_Traded")]
        public long? Grade2_MaterialsTraded { get; set; }

        [JsonProperty("Grade_3_Materials_Traded")]
        public long? Grade3_MaterialsTraded { get; set; }

        [JsonProperty("Grade_4_Materials_Traded")]
        public long? Grade4_MaterialsTraded { get; set; }

        [JsonProperty("Grade_5_Materials_Traded")]
        public long? Grade5_MaterialsTraded { get; set; }
    }

    public class Mining
    {
        [JsonProperty("Mining_Profits")]
        public long MiningProfits { get; set; }

        [JsonProperty("Quantity_Mined")]
        public long QuantityMined { get; set; }

        [JsonProperty("Materials_Collected")]
        public long MaterialsCollected { get; set; }
    }

    public class Multicrew
    {
        [JsonProperty("Multicrew_Time_Total")]
        public long MulticrewTimeTotal { get; set; }

        [JsonProperty("Multicrew_Gunner_Time_Total")]
        public long MulticrewGunnerTimeTotal { get; set; }

        [JsonProperty("Multicrew_Fighter_Time_Total")]
        public long MulticrewFighterTimeTotal { get; set; }

        [JsonProperty("Multicrew_Credits_Total")]
        public long MulticrewCreditsTotal { get; set; }

        [JsonProperty("Multicrew_Fines_Total")]
        public long MulticrewFinesTotal { get; set; }
    }

    public class Passengers
    {
        [JsonProperty("Passengers_Missions_Accepted")]
        public long? PassengersMissionsAccepted { get; set; }

        [JsonProperty("Passengers_Missions_Disgruntled")]
        public long? PassengersMissionsDisgruntled { get; set; }

        [JsonProperty("Passengers_Missions_Bulk")]
        public long? PassengersMissionsBulk { get; set; }

        [JsonProperty("Passengers_Missions_VIP")]
        public long? PassengersMissionsVip { get; set; }

        [JsonProperty("Passengers_Missions_Delivered")]
        public long? PassengersMissionsDelivered { get; set; }

        [JsonProperty("Passengers_Missions_Ejected")]
        public long? PassengersMissionsEjected { get; set; }
    }

    public class SearchAndRescue
    {
        [JsonProperty("SearchRescue_Traded")]
        public long SearchRescueTraded { get; set; }

        [JsonProperty("SearchRescue_Profit")]
        public long SearchRescueProfit { get; set; }

        [JsonProperty("SearchRescue_Count")]
        public long SearchRescueCount { get; set; }
    }

    public class Smuggling
    {
        [JsonProperty("Black_Markets_Traded_With")]
        public long BlackMarketsTradedWith { get; set; }

        [JsonProperty("Black_Markets_Profits")]
        public long BlackMarketsProfits { get; set; }

        [JsonProperty("Resources_Smuggled")]
        public long ResourcesSmuggled { get; set; }

        [JsonProperty("Average_Profit")]
        public double AverageProfit { get; set; }

        [JsonProperty("Highest_Single_Transaction")]
        public long HighestSingleTransaction { get; set; }
    }

    public class TgEncounters
    {
        [JsonProperty("TG_ENCOUNTER_IMPRINT")]
        public long? TgEncounterImprint { get; set; }

        [JsonProperty("TG_ENCOUNTER_TOTAL")]
        public long TgEncounterTotal { get; set; }

        [JsonProperty("TG_ENCOUNTER_TOTAL_LAST_SYSTEM")]
        public string TgEncounterTotalLastSystem { get; set; }

        [JsonProperty("TG_ENCOUNTER_TOTAL_LAST_TIMESTAMP")]
        public string TgEncounterTotalLastTimestamp { get; set; }

        [JsonProperty("TG_ENCOUNTER_TOTAL_LAST_SHIP")]
        public string TgEncounterTotalLastShip { get; set; }

        [JsonProperty("TG_SCOUT_COUNT")]
        public long TgScoutCount { get; set; }
    }

    public class Trading
    {
        [JsonProperty("Markets_Traded_With")]
        public long MarketsTradedWith { get; set; }

        [JsonProperty("Market_Profits")]
        public long MarketProfits { get; set; }

        [JsonProperty("Resources_Traded")]
        public long ResourcesTraded { get; set; }

        [JsonProperty("Average_Profit")]
        public double AverageProfit { get; set; }

        [JsonProperty("Highest_Single_Transaction")]
        public long HighestSingleTransaction { get; set; }
    }

}
