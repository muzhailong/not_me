using System.Collections.Generic;
using UnityEngine;
using Vectrosity;
using my;

public class PathController : MonoBehaviour {
    public Texture2D tex2d;
    public float lineWidth = 2.0f;
    private PathPlayer pp;
    private PathPlayer pp2;
    private PathPlayer pp3;


    private GameObject map;

    private VectorLine vl, vl2,vl3;
    void Start ()
    {
        map = GameObject.Find("pathPlayer");
        vl = new VectorLine("DrawnLine", new List<Vector2>(), tex2d, lineWidth, LineType.Continuous, Joins.Weld);
        vl.endPointsUpdate = 2;
        vl.SetColor(Color.white);

        vl2= new VectorLine("Dots", new List<Vector2>(), 2, LineType.Points);
        vl2.SetColor(Color.red);

        vl3 = new VectorLine("Dots2", new List<Vector2>(), tex2d, lineWidth, LineType.Continuous, Joins.Weld);
        vl3.SetColor(Color.red);
    }
	
	void Update ()
    {
        if (pp != null)
        {
            pp.draw();
        }

        if (pp2 != null)
        {
            pp2.draw();
        }

        if (pp3 != null)
        {
            pp3.draw();
        }

    }

    public void ready(string path)
    {
        vl.points2.Clear();
        vl2.points2.Clear();
        vl3.points2.Clear();

        PointEntry centerPoint=new PointEntry(map.transform.position.x,map.transform.position.y);
        KeyValuePair<float, float> rect = new KeyValuePair<float, float>(Config.PATH_PLAYER_WIDTH, Config.PATH_PLAYER_HEIGHT);
        PointEntryCollections pec = PointEntryCollections.build(path).compressChange(centerPoint, rect);

        pp = new PathPlayer(pec,vl,Color.white);
        pp.ready();
        pp.forward();

        pp2 = new PathPlayer(pec, vl2,Color.red, PathPlayer.LINE_TYPE.PART);
        //pp2 = new PathPlayer(pec, vl2,Color.red);
        pp2.ready();

        pp3= new PathPlayer(pec, vl3, Color.red);
        pp3.ready();
    }

    public void pause()
    {
        pp.pause();
        pp2.pause();
        pp3.pause();
    }

    public void play()
    {
        pp.play();
        pp2.play();
        pp3.play();
    }

    public List<PointEntry> acquirePointsList()
    {
        return pp2.copyPoints();
    }
}
