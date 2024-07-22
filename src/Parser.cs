using HtmlAgilityPack;
using System.Text.RegularExpressions;

public class Parser(){


    public List<string> ParseLyrics(string ?rawhtml){
        HtmlDocument htmlSnippet = new();
        htmlSnippet.LoadHtml(rawhtml);

        List<string> lyrics = [];

        HtmlNodeCollection lyricNodes = htmlSnippet.DocumentNode.SelectNodes("//div[@data-lyrics-container='true']");
        if(lyricNodes != null){
            var italicNodes = lyricNodes.Descendants("i").ToList();
            foreach (var node in italicNodes){
                node.Remove();
            }
            foreach (HtmlNode node in lyricNodes.Descendants()){        
                if(node.NodeType == HtmlNodeType.Text && !node.InnerText.Contains('[') && !node.InnerText.Contains(']')){
                    foreach (string word in node.InnerText.Split(' ', '-')){ 
                        string decodedWord = System.Net.WebUtility.HtmlDecode(word).Trim();
                        string cleanedWord = Regex.Replace(decodedWord, "[^a-zA-Z0-9]+", "");
                        if(cleanedWord != ""){
                            lyrics.Add(cleanedWord);
                        }
                    }
                }         
            }
        }

        return lyrics;
    }


}