using HtmlAgilityPack;
using System.Text.RegularExpressions;

public class Parser(){


    public List<string> ParseLyrics(string ?rawhtml){
        HtmlDocument htmlSnippet = new();
        htmlSnippet.LoadHtml(rawhtml);

        List<string> lyrics = [];

        HtmlNodeCollection lyricNodes = htmlSnippet.DocumentNode.SelectNodes("//div[@data-lyrics-container='true']");
        if(lyricNodes != null){
            bool inQuotes = false;
            foreach (HtmlNode node in lyricNodes.Descendants()){  
                if(node.NodeType == HtmlNodeType.Text){
                    foreach (string word in node.InnerText.Split(' ', '-')){
                        if(word.Contains('[')){
                            if(!word.Contains(']')){
                                inQuotes = true;
                            }
                        }else if (inQuotes && word.Contains(']')){
                            inQuotes = false;
                        }else if (!inQuotes){
                            string decodedWord = System.Net.WebUtility.HtmlDecode(word).Trim();
                            string cleanedWord = Regex.Replace(decodedWord, "[^a-zA-Z0-9]+", "");
                            if(cleanedWord != ""){
                                lyrics.Add(cleanedWord);
                            }
                        }
                    }
                }         
            }
        }

        return lyrics;
    }

    public string ParseSong(string ?rawhtml){
        HtmlDocument htmlSnippet = new();
        htmlSnippet.LoadHtml(rawhtml);

        HtmlNode songNode = htmlSnippet.DocumentNode.SelectSingleNode("//h1[starts-with(@class, 'SongHeaderdesktop__Title')]");
        if(songNode != null){
            foreach (HtmlNode node in songNode.Descendants()){ 
                if(node.NodeType == HtmlNodeType.Text){
                    return node.InnerText;
                }         
            }
        }

        return "Unknown";
    }

    public string ParseArtist(string ?rawhtml){
        HtmlDocument htmlSnippet = new();
        htmlSnippet.LoadHtml(rawhtml);

        HtmlNode artistNode = htmlSnippet.DocumentNode.SelectSingleNode("//div[starts-with(@class, 'HeaderArtistAndTracklistdesktop__ListArtists')]");
        if(artistNode != null){
            foreach (HtmlNode node in artistNode.Descendants()){ 
                if(node.NodeType == HtmlNodeType.Text){
                    return node.InnerText;
                }         
            }
        }

        return "Unknown";
    }

}