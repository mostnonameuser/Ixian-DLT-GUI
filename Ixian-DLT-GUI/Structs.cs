using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ixian_DLT_GUI
{
    public class GitAnswer
    {
        [JsonProperty("url")]
        public string? url { get; set; }
        [JsonProperty("assets_url")]
        public string? assetsUrl { get; set; }
        [JsonProperty("upload_url")]
        public string? uploadUrl { get; set; }
        [JsonProperty("html_url")]
        public string? htmlUrl { get; set; }
        [JsonProperty("id")]
        public string? id { get; set; }
        [JsonProperty("author")]
        public Author author { get; set; }
        [JsonProperty("node_id")]
        public string? nodeId { get; set; }
        [JsonProperty("tag_name")]
        public string? tagName { get; set; }
        [JsonProperty("target_commitish")]
        public string? targetCommitish { get; set; }
        [JsonProperty("name")]
        public string? name { get; set; }
        [JsonProperty("draft")]
        public string? draft { get; set; }
        [JsonProperty("prepelease")]
        public string? prerelease { get; set; }
        [JsonProperty("created_at")]
        public string? createdAt { get; set; }
        [JsonProperty("published_at")]
        public string? publishedAt { get; set; }
        [JsonProperty("assets")]
        public Assets[] assets { get; set; }
        [JsonProperty("tarbal_url")]
        public string? tarbalUrl { get; set; }
        [JsonProperty("zipbal_url")]
        public string? zipbalUrl { get; set; }
    }
    public class Author
    {
        [JsonProperty("login")]
        public string? login { get; set; }
        [JsonProperty("id")]
        public string? id { get; set; }
        [JsonProperty("node_id")]
        public string? nodeId { get; set; }
        [JsonProperty("avatar_url")]
        public string? avatarUrl { get; set; }
        [JsonProperty("gavatar_id")]
        public string? gavatarId { get; set; }
        [JsonProperty("url")]
        public string? url { get; set; }
        [JsonProperty("html_url")]
        public string? htmlUrl { get; set; }
        [JsonProperty("followers_url")]
        public string? followersUrl { get; set; }
        [JsonProperty("following_url")]
        public string? followingUrl { get; set; }
        [JsonProperty("gists_url")]
        public string? gistsUrl { get; set; }
        [JsonProperty("starred_url")]
        public string? starredUrl { get; set; }
        [JsonProperty("subscriptions_url")]
        public string? subscriptionsUrl { get; set; }
        [JsonProperty("organizations_url")]
        public string? organizationsUrl { get; set; }
        [JsonProperty("repos_url")]
        public string? reposUrl { get; set; }
        [JsonProperty("events_url")]
        public string? eventsUrl { get; set; }
        [JsonProperty("recieved_events_url")]
        public string? recievedEventsUrl { get; set; }
        [JsonProperty("type")]
        public string? type { get; set; }
        [JsonProperty("site_admin")]
        public string? siteAdmin { get; set; }
    }
    public class Assets
    {
        [JsonProperty("url")]
        public string? url { get; set; }
        [JsonProperty("id")]
        public string? id { get; set; }
        [JsonProperty("node_id")]
        public string? nodeId { get; set; }
        [JsonProperty("name")]
        public string? name { get; set; }
        [JsonProperty("label")]
        public string? label { get; set; }
        [JsonProperty("uploader")]
        public Uploader uploader { get; set; }
        [JsonProperty("content_type")]
        public string? contentType { get; set; }
        [JsonProperty("state")]
        public string? state { get; set; }
        [JsonProperty("size")]
        public string? size { get; set; }
        [JsonProperty("download_count")]
        public string? downloadCount { get; set; }
        [JsonProperty("created_at")]
        public string? createdAt { get; set; }
        [JsonProperty("updated_at")]
        public string? updatedAt { get; set; }
        [JsonProperty("browser_download_url")]
        public string? browserDownloadUrl { get; set; }
    }
    public class Uploader
    {
        [JsonProperty("login")]
        public string? login { get; set; }
        [JsonProperty("id")]
        public string? id { get; set; }
        [JsonProperty("node_id")]
        public string? nodeId { get; set; }
        [JsonProperty("avatar_url")]
        public string? avatarUrl { get; set; }
        [JsonProperty("gavatar_id")]
        public string? gavatarId { get; set; }
        [JsonProperty("url")]
        public string? url { get; set; }
        [JsonProperty("html_url")]
        public string? htmlUrl { get; set; }
        [JsonProperty("followers_url")]
        public string? followersUrl { get; set; }
        [JsonProperty("following_url")]
        public string? followingUrl { get; set; }
        [JsonProperty("gists_url")]
        public string? gistsUrl { get; set; }
        [JsonProperty("starred_url")]
        public string? starredUrl { get; set; }
        [JsonProperty("subscriptions_url")]
        public string? subscriptionsUrl { get; set; }
        [JsonProperty("organizations_url")]
        public string? organizationsUrl { get; set; }
        [JsonProperty("repos_url")]
        public string? reposUrl { get; set; }
        [JsonProperty("events_url")]
        public string? eventsUrl { get; set; }
        [JsonProperty("recieved_events_url")]
        public string? recievedEventsUrl { get; set; }
        [JsonProperty("type")]
        public string? type { get; set; }
        [JsonProperty("site_admin")]
        public string? siteAdmin { get; set; }
    }

    public class Balance
    {
        [JsonProperty("result")]
        public string? result { get; set; }
        [JsonProperty("error")]
        public string? error { get; set; }
        [JsonProperty("id")]
        public string? id { get; set; }
    }
    public class Status
    {
        [JsonProperty("result")]
        public IXIResult? result { get; set; }
        [JsonProperty("error")]
        public string? error { get; set; }
        [JsonProperty("id")]
        public string? id { get; set; }
    }
    public class IXIResult
    {
        [JsonProperty("Core Version")]
        public string? coreVer { get; set; }
        [JsonProperty("Node Version")]
        public string? nodeVer { get; set; }
        [JsonProperty("Network type")]
        public string? netType { get; set; }
        [JsonProperty("My time")]
        public int? myTime { get; set; }
        [JsonProperty("Network time difference")]
        public int? networkTimeDiff { get; set; }
        [JsonProperty("Real network time difference")]
        public int? realNetTimeDiff { get; set; }
        [JsonProperty("My External IP")]
        public string? exip { get; set; }
        [JsonProperty("My Listening Port")]
        public int? dltPort { get; set; }
        [JsonProperty("Node Deprecation Block Limit")]
        public int? deprTime { get; set; }
        [JsonProperty("Update")]
        public string update { get; set; }
        [JsonProperty("DLT Status")]
        public string? status { get; set; }
        [JsonProperty("Core Status")]
        public int? coreStatus { get; set; }
        [JsonProperty("Block Processor Status")]
        public string? blockProcStatus { get; set; }
        [JsonProperty("Block Height")]
        public int? curblock { get; set; }
        [JsonProperty("Block Version")]
        public int? blockVer { get; set; }
        [JsonProperty("Block Signature Count")]
        public int? blockSigCount { get; set; }
        [JsonProperty("Block Total Signer Difficulty")]
        public string? blTotalSigDiff { get; set; }
        [JsonProperty("Block generated seconds ago")]
        public int? blockGenSecAgo { get; set; }
        [JsonProperty("Network Block Height")]
        public int? netBlock { get; set; }
        [JsonProperty("Node Type")]
        public string? nodeType { get; set; }
        [JsonProperty("Connectable")]
        public bool? connectable { get; set; }
        [JsonProperty("Required Consensus")]
        public int? reqCons { get; set; }
        [JsonProperty("Signer Difficulty")]
        public string? sigDiff { get; set; }
        [JsonProperty("Signer Bits")]
        public string? sigBits { get; set; }
        [JsonProperty("Signer Hashrate")]
        public int? spow { get; set; }
        [JsonProperty("Signer Last PoW Solution")]
        public SpowSolution? lastSpowSolution { get; set; }
        [JsonProperty("Signer Active PoW Solution")]
        public SpowSolution? activeSpowSolution { get; set; }
        [JsonProperty("Wallets")]
        public int? wallets{ get; set; }
        [JsonProperty("Presences")]
        public int? presences { get; set; }
        [JsonProperty("Supply")]
        public string? supply { get; set; }
        [JsonProperty("Applied TX Count")]
        public int? apTxCount { get; set; }
        [JsonProperty("Unapplied TX Count")]
        public int? unTxCount { get; set; }
        [JsonProperty("Masters")]
        public int? masters { get; set; }
        [JsonProperty("Relays")]
        public int? relays { get; set; }
        [JsonProperty("Clients")]
        public int? clients { get; set; }
        [JsonProperty("Queues")]
        public Queues? qeues { get; set; }
        [JsonProperty("WS Checksum")]
        public string? wsChecksum { get; set; }
        [JsonProperty("Blockchain Scanning Active")]
        public bool? bchScanActive { get; set; }
        [JsonProperty("Activity Scanning Last Block")]
        public int? actScanLastBlock { get; set; }
        [JsonProperty("Network Clients")]
        public string[]? netClients { get; set; }
        [JsonProperty("Network Servers")]
        public string[]? netServers { get; set; }
    }
    public class Queues
    {
        [JsonProperty("RcvLow")]
        public int? rcvLow { get; set; }
        [JsonProperty("RcvMedium")]
        public int? rcvMedium { get; set; }
        [JsonProperty("RcvHigh")]
        public int? rcvHigh { get; set; }
        [JsonProperty("SendClients")]
        public int? sendClients { get; set; }
        [JsonProperty("SendServers")]
        public int? sendServers { get; set; }
        [JsonProperty("Pending Transactions")]
        public int? pendTx { get; set; }
        [JsonProperty("Storage")]
        public int? storage { get; set; }
        [JsonProperty("Inventory")]
        public int? inventory { get; set; }
        [JsonProperty("Inventory Processed")]
        public int? inventoryProcessed { get; set; }
        [JsonProperty("Activity")]
        public int? activity { get; set; }
    }
    public class SpowSolution
    {
        [JsonProperty("blockNum")]
        public int? blockNum { get; set; }
        [JsonProperty("solution")]
        public string? solution { get; set; }
        [JsonProperty("checksum")]
        public string? checksum { get; set; }
        [JsonProperty("difficulty")]
        public string? difficulty { get; set; }
        [JsonProperty("bits")]
        public string? bits { get; set; }
    }
}
