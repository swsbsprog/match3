using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    static public BlockManager instance;
    Dictionary<Vector2Int, Block> blockMap = new Dictionary<Vector2Int, Block>();

    private void Awake() => instance = this;
    private void OnDestroy() => instance = null;

    private void Start() => blockMap = BlockPlacer.instance.blockGoList
        .ToDictionary(x => x.Pos);

    internal void Swap(Block startBlock, int moveX, int moveY)
    {
        Vector2Int newPos = startBlock.Pos + new Vector2Int(moveX, moveY);
        // ���� Ȯ��.
        if (CanMove(newPos) == false)
            return;

        var endBlock = blockMap[newPos];

        SwapPosition(startBlock, endBlock);
    }

    private bool CanMove(Vector2Int newPos)
    {
        if (newPos.x < 0 || newPos.y < 0)
        {
            UILoger.Log("�ּҰ� ���� �۴�. ������ �Ұ�"); return false;
        }

        if (newPos.x >= BlockPlacer.instance.countX ||
            newPos.y >= BlockPlacer.instance.countY)
        {
            UILoger.Log("�ִ밪 ���� ũ��. ������ �Ұ�"); return false;
        }

        return true;
    }

    private void SwapPosition(Block startBlock, Block endBlock)
    {
        var tempPos = startBlock.transform.position;
        startBlock.transform.position = endBlock.transform.position;
        endBlock.transform.position = tempPos;
        blockMap[endBlock.Pos] = endBlock;
        blockMap[startBlock.Pos] = startBlock;
    }
}
