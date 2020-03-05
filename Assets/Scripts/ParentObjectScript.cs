using UnityEngine;

public class ParentObjectScript : MonoBehaviour
{

    private int counter;

    private float lastValue;
    private float scaleCounter;

    private bool firstClick;

    public GameObject insideObject;
    public GameObject outsideObject;
    public GameObject particleConfetti;

    void Update()
    {
        DragHexagon();
        if (!GameControlScript.instance.isNotHexNull && firstClick)
            LocalScaleReduce();
        else if (GameControlScript.instance.isNotHexNull)
            HexagonObjectControl();

    }

    //Objemiz gameover durumunda aşağıdaki koşulları 
    private void HexagonObjectControl()
    {
        if (GameControlScript.instance.isNotHexNull)
        {
            if (outsideObject.transform.localScale.x > (insideObject.transform.localScale.x / 3) + 0.01f)
            {
                GetComponent<Explosion>().explode();
                GameControlScript.instance.gameOver = true;
            }
            else if (!GameControlScript.instance.gameOver)
            {
                if ((outsideObject.transform.localScale.x <= (insideObject.transform.localScale.x / 3) + 0.01f) && insideObject.transform.localScale.x <= 3.3f)
                {
                    insideObject.transform.localRotation = Quaternion.Euler(0, 0, 0);

                    insideObject.transform.localScale = new Vector3(insideObject.transform.localScale.x + 0.4f,
                        insideObject.transform.localScale.y, insideObject.transform.localScale.z + 0.4f);

                    if (counter == 0)
                    {
                        counter++;
                        Instantiate(particleConfetti, particleConfetti.transform.position, Quaternion.identity, transform);
                        GameControlScript.instance.passText.SetActive(true);
                        GameControlScript.instance.passText.SetActive(true);
                        outsideObject.GetComponent<MeshRenderer>().material.color = insideObject.GetComponent<MeshRenderer>().material.color;
                    }

                }
                else if (GameControlScript.instance.nextTabLevel)
                {
                    GameControlScript.instance.levelCounter++;
                    PlayerPrefs.SetInt("LevelCount", GameControlScript.instance.levelCounter);
                    GameControlScript.instance.newReadyHex = true;
                    GameControlScript.instance.passText.SetActive(false);
                    GameControlScript.instance.nextTabLevel = false;
                }
            }
        }
    }

    //Objeyi kaydırarak rotasyona bağlamayı sağlıyor.
    private void DragHexagon()
    {
        if (GameControlScript.instance.isTouch && lastValue != GameControlScript.instance.lastPos && !GameControlScript.instance.isNotHexNull)
        {
            firstClick = true;
            if (lastValue == GameControlScript.instance.lastPos)
                GameControlScript.instance.startPos = lastValue;
            GameControlScript.instance.animationSwipe.SetActive(false); // Burada gameControldeki animasyonu kapatmak için yapıyoruz

            if (lastValue > GameControlScript.instance.lastPos && Mathf.Abs(lastValue - GameControlScript.instance.lastPos) > 0.5f)
            {
                lastValue = GameControlScript.instance.lastPos;
                insideObject.transform.Rotate(new Vector3(0, 1f, 0));
            }
            else if (lastValue < GameControlScript.instance.lastPos && Mathf.Abs(lastValue - GameControlScript.instance.lastPos) > 0.5f)
            {
                lastValue = GameControlScript.instance.lastPos;
                insideObject.transform.Rotate(new Vector3(0, -1f, 0));
            }
        }
    }

    // Burada hexagon etrafındaki çercevenin boyutunu scale'i küçültüyoruz!
    private void LocalScaleReduce()
    {
        if (outsideObject.transform.localScale.x != (insideObject.transform.localScale.x / 3) ||
            outsideObject.transform.localScale.z != (insideObject.transform.localScale.z / 3) )
        {
            if (Time.deltaTime > 0)
                scaleCounter += 0.0025f; // Buraya bir değişken eklenerek küçülme hızı arttırılabilecek!

            Vector3 changeScale = outsideObject.transform.localScale;
            changeScale.x -= (scaleCounter);
            changeScale.z -= (scaleCounter);

            changeScale.x = Mathf.Floor(changeScale.x * 1000f) / 1000f; // sayıları tam olarak virgülden sonra iki basamak olması için aşağı yuvarlıyoruz!
            changeScale.z = Mathf.Floor(changeScale.z * 1000f) / 1000f;
            outsideObject.transform.localScale = new Vector3(changeScale.x, outsideObject.transform.localScale.y, changeScale.z);

            scaleCounter = 0;
        }
    }

}









//else if ((outsideObject.transform.localScale.x <= (insideObject.transform.localScale.x / 3) + 0.025f) && insideObject.transform.localScale.x <= 3.3f)
//{
//    insideObject.transform.localRotation = Quaternion.Euler(0, 0, 0);

//    insideObject.transform.localScale = new Vector3(insideObject.transform.localScale.x + 0.3f,
//        insideObject.transform.localScale.y, insideObject.transform.localScale.z + 0.3f);
//    GameControlScript.instance.passText.SetActive(true);

//    outsideObject.GetComponent<MeshRenderer>().material.color = insideObject.GetComponent<MeshRenderer>().material.color;
//}


//if (insideObject.transform.localScale.x >= 3.3f && GameControlScript.instance.nextTabLevel)
//{
//    GameControlScript.instance.newReadyHex = true;
//    GameControlScript.instance.passText.SetActive(false);
//    GameControlScript.instance.levelCounter++;
//    PlayerPrefs.SetInt("LevelCount", GameControlScript.instance.levelCounter);
//    GameControlScript.instance.nextTabLevel = false;
//}