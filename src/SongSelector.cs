using HtmlAgilityPack;

public class SongSelector(){

  private List<string> songList = File.ReadAllLines("./src/song_list.txt").ToList();
  
  public string SelectRandomSong(){
    if(songList.Count != 0){
      Random r = new Random();
      int rInt = r.Next(0, songList.Count);
      return songList[rInt];
    }
    return "";
  }

  public List<string> GetAllSongs(){
    return songList;
  }

}