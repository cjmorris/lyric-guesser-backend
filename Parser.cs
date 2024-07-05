using HtmlAgilityPack;
using System.Text.RegularExpressions;

public class Parser(){


    public List<string> ParseLyrics(string rawhtml){
        HtmlDocument htmlSnippet = new HtmlDocument();
        htmlSnippet.LoadHtml(rawhtml);

        List<string> lyrics = new List<string>();

        HtmlNode? lyricsNode = null;
        foreach (HtmlNode child in htmlSnippet.DocumentNode.Descendants()){
            if(child.InnerHtml.Contains("any third-party lyrics provider")){
                lyricsNode = child;
            }
        }

        if(lyricsNode != null){
            foreach (HtmlNode child in lyricsNode.ParentNode.ChildNodes){
                if(!(child.InnerHtml.Contains("<") || child.InnerHtml.Contains("(") || child.InnerHtml.Contains(")"))){
                    foreach (string word in child.InnerHtml.Split(' ', '-')){
                        string cleanedWord = word.Trim();
                        if(cleanedWord != ""){
                            lyrics.Add(Regex.Replace(cleanedWord, "[^a-zA-Z0-9]+", ""));
                        }
                    }
                }         
            }
        }

        return lyrics;
    }


}