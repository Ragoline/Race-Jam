using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gift : MonoBehaviour
{
    [SerializeField] private Image _giftBox;
    [SerializeField] private Text _gearsNumber;
    [SerializeField] private Text _comeBack;

    public void TakeGift()
    {
        if (PlayerPrefs.HasKey("Gift"))
        {
            // ���� ������ ���� �� ����, ������� ���������� (ReceiveGift())
        }
        else
        {
            ReceiveGift();
        }
    }

    private void ReceiveGift()
    {
        // todo �������� ����, ��� ������� ����������
    }
}
