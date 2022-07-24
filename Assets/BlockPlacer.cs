using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockPlacer : MonoBehaviour
{
    static public BlockPlacer instance;
    
    [System.Serializable]
    public class BlockInfo
    {
        public int type;
        public Sprite sprite;
    }
    public List<BlockInfo> sprites;
    public Block baseblock;
    public int countX = 7;
    public int countY = 5;
    private void Awake() => instance = this;
    private void OnDestroy() => instance = null;


    public List<Block> blockGoList = new List<Block>();
    [ContextMenu("¹èÄ¡")]
    void PlaceBlock()
    {
        blockGoList.ForEach( x =>
        {
            if(Application.isPlaying)
                Destroy(x.gameObject);
            else
                DestroyImmediate(x.gameObject);
        });
        blockGoList.Clear();

        for (int x = 0; x < countX; x++)
        {
            for (int y = 0; y < countY; y++)
            {
                BlockInfo blockInfo = sprites.OrderBy(x => Random.value).First();
                var go = Instantiate(baseblock, new Vector3(x, y), Quaternion.Euler(0, 0, 0));
                go.SetInfo(blockInfo);
                go.name = $"x:{x} y:{y}";
                blockGoList.Add(go);
            }
        }
    }
}
