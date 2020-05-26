using UnityEngine;

public class ItemBlockManager : MonoBehaviour
{
    public MenuManager menuManager;
    public ItemBlock[] itemblocks;
    
    public void ChangeStateAllItems()
    {
        for (int i = 0; i <= itemblocks.Length-1; i++)
        {
            itemblocks[i].ChangeStateItemBlock();
        }
        menuManager.SetCurrencyPoints();
    }
}
