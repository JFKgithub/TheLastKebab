using UnityEngine;

public class dialogScript : MonoBehaviour
{
    public void storyA()
    {
        CallDialogManager callDialogManager = FindFirstObjectByType<CallDialogManager>();
        callDialogManager.StartDialog("Uncle", new string[]
        {
            //Dialog
            "This is your uncle speaking.", // 1
            "You need to listen carefully.",// 2
            "The kebab shop is cursed.",// 3
            "Every night at midnight, they come.",// 4
            "They’re not human anymore.",// 5
            "They’ll demand food, and if you don’t feed them fast enough...",// 6
            "...you’ll become the meal.",// 7
            "Stay calm, don’t panic.",// 8
            "Fear makes them stronger.",// 9
            "I’m sorry, kid... I should’ve warned you sooner."// 10
        },
        new float[]
        {
            //Type Speed for each sentence.
            0.05f, // 1
            0.07f, // 2
            0.07f, // 3
            0.09f, // 4
            0.1f,  // 5
            0.09f, // 6
            0.12f, // 7
            0.08f, // 8
            0.1f,  // 9
            0.12f  // 10
        });
    }
}
