using DG.Tweening;
using System;
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
        // 범위 확인.
        if (CanMove(newPos) == false)
            return;

        var endBlock = blockMap[newPos];

        SwapPosition(startBlock, endBlock);
    }

    int MaxX => BlockPlacer.instance.countX;
    int MaxY => BlockPlacer.instance.countY;
    private bool CanMove(Vector2Int newPos)
    {
        if (newPos.x < 0 || newPos.y < 0)
        {
            UILoger.Log("최소값 보다 작다. 움직임 불가"); return false;
        }

        if (newPos.x >= MaxX || newPos.y >= MaxY)
        {
            UILoger.Log("최대값 보다 크다. 움직임 불가"); return false;
        }

        return true;
    }

    public float duration = 0.2f;
    public Ease ease = Ease.InOutSine;
    private void SwapPosition(Block aBlock, Block bBlock)
    {
        var bEndPos = aBlock.transform.position;
        var aEndPos = bBlock.transform.position;

        aBlock.transform.DOMove(aEndPos, duration)
            .SetEase(ease);
        bBlock.transform.DOMove(bEndPos, duration)
            .SetEase(ease)
            .OnComplete(OnCompleteMove);
        blockMap[bEndPos.ToVector2Int()] = bBlock;
        blockMap[aEndPos.ToVector2Int()] = aBlock;
    }

    private void OnCompleteMove()
    {
        print("이동 완료");
        CheckMatch();
    }

    public List<List<Block>> sameBlockList = new List<List<Block>>();
    private void CheckMatch()
    {
        sameBlockList.Clear();
        CheckMatchX();
        CheckMatchY();
        //CheckMatchRectangle();

        DeleteMatchBlockList();
    }

    private void CheckMatchY()
    {
        List<Block> sameBlocks = new List<Block>();
        for (int x = 0; x < MaxX; x++)
        {
            Block previousBlock = null;
            sameBlocks.Clear();
            for (int y = 0; y < MaxY; y++)
            {
                var currentBlock = blockMap[new Vector2Int(x, y)];
                if (previousBlock != null
                    && previousBlock.type == currentBlock.type)
                {
                    if (sameBlocks.Count == 0)
                        sameBlocks.Add(previousBlock);
                    sameBlocks.Add(currentBlock);
                }
                else
                {
                    if (sameBlocks.Count >= 3)
                        sameBlockList.Add(sameBlocks.ToList());
                    sameBlocks.Clear();
                }

                previousBlock = currentBlock;
            }

            if (sameBlocks.Count >= 3)
                sameBlockList.Add(sameBlocks.ToList());
            sameBlocks.Clear();
        }
    }

    private void CheckMatchX()
    {
        List<Block> sameBlocks = new List<Block>();
        for (int y = 0; y < MaxY; y++)
        {
            Block previousBlock = null;
            sameBlocks.Clear();
            for (int x = 0; x < MaxX; x++)
            {
                var currentBlock = blockMap[new Vector2Int(x, y)];
                if (previousBlock != null 
                    && previousBlock.type == currentBlock.type)
                {
                    if(sameBlocks.Count == 0)
                        sameBlocks.Add(previousBlock);
                    sameBlocks.Add(currentBlock);
                }
                else
                {
                    if(sameBlocks.Count >= 3)
                        sameBlockList.Add(sameBlocks.ToList());
                    sameBlocks.Clear();
                }

                previousBlock = currentBlock;
            }
        }
    }
    private void DeleteMatchBlockList()
    {
        foreach (var sameGroup in sameBlockList)
        {
            foreach (var block in sameGroup)
            {
                blockMap[block.Pos] = null;
                Destroy(block.gameObject);
            }
        }
    }

}
