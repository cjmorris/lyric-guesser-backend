using HtmlAgilityPack;
using System.Text.RegularExpressions;

public class Parser(){


    public List<string> ParseLyrics(string ?rawhtml){
        HtmlDocument htmlSnippet = new();
        htmlSnippet.LoadHtml(rawhtml);

        List<string> lyrics = [];

        HtmlNodeCollection lyricNodes = htmlSnippet.DocumentNode.SelectNodes("//div[@data-lyrics-container='true']");
        if(lyricNodes != null){
            foreach (HtmlNode node in lyricNodes.Descendants()){        
                if(node.NodeType == HtmlNodeType.Text){
                    bool inQuotes = false;
                    foreach (string word in node.InnerText.Split(' ', '-')){
                        if(word.Contains('[')){
                            inQuotes = true;
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


}