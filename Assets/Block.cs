using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    void OnMouseDown()
    {
       //Destroy(gameObject);
        previousPos = Input.mousePosition;
    }

    Vector3 previousPos;
    float nextEnableTouchTime;

    public Vector2Int Pos => transform.position.ToVector2Int();

    void OnMouseDrag()
    {
        if (nextEnableTouchTime > Time.time)
            return;

        Vector3 currentPos = Input.mousePosition;
        Vector3 moveDelta = currentPos - previousPos;
        float absX = Mathf.Abs(moveDelta.x);
        float absY = Mathf.Abs(moveDelta.y);
        //print(moveDelta);
        if(absX == absY)
        {
            //print("������ �����Ұ�");
        }
        else if (absX > absY)
        {
            if (moveDelta.x > 0)
                Move(1, 0);// print("������ �����̱�");
            else
                Move(-1, 0);// ("���� �����̱�");
            nextEnableTouchTime = Time.time + 0.5f;
        }
        else
        {
            if (moveDelta.y > 0)
                Move(0, 1);//print("�� �����̱�");
            else
                Move(0, -1);//print("�Ʒ� �����̱�");
            nextEnableTouchTime = Time.time + 0.5f;
        }
        previousPos = currentPos;
    }

    private void Move(int moveX, int moveY)
    {
        BlockManager.instance.Swap(this, moveX, moveY);
    }
}
