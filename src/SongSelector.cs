using HtmlAgilityPack;

public class SongSelector(){

  private string[] songList = File.ReadAllLines("./src/song_list.txt");
  
  public string SelectRandomSong(){
    if(songList.Length != 0){
      Random r = new Random();
      int rInt = r.Next(0, songList.Length);
      return songList[rInt];
    }
    return "";
  }

  public string[] GetAllSongs(){
    return songList;
  }

}