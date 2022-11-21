using UnityEngine;

public class Transformer : MonoBehaviour
{
    public Vector3 translationRate;
    public Vector3 rotationRate;
    public Vector3 childRotationRate;
    public string key;

    void Update()
    {

        // #### Place an instance of the transformer class into Sphere_2 (key t), finger_small 0 (key h), finger_small_1 (key h), finger_small_2 (key h), smaller_finger, smaller_finger_1, smaller_finger_2 (key h)
        //Vector3 rotationVec;
        float rot = 0.0f;
        float otherRot = 0.0f;
        if (Input.GetKey(KeyCode.LeftShift))
        {   
            // if the current gameobject is sphere 2:
            if (key == "t") {

            // T to rotate sphere 2 around Y-axis
            if (Input.GetKey(KeyCode.T))
            {
                rot -= 30.0f * Time.deltaTime;
                rotationRate = new Vector3(0,rot,0);
                transform.localRotation = transform.localRotation * Quaternion.Euler(rotationRate);
            }

            // R to raise sphere 2 
            if (Input.GetKey(KeyCode.R))
            {
                translationRate = new Vector3(0, -0.5f, 0);
                transform.Translate(translationRate * Time.deltaTime);
            }

            // U to rotate sphere 2 around Z-Axis ( and rotate sphere 4 in reverse to stay parralel )
            if (Input.GetKey(KeyCode.U))
            {
                rot -= 20.0f * Time.deltaTime;
                rotationRate = new Vector3(0,0,rot);
                transform.localRotation = transform.localRotation * Quaternion.Euler(rotationRate);
                //now rotate sphere 4 in reverse
                otherRot += 20.0f * Time.deltaTime;
                childRotationRate = new Vector3(0,0,otherRot);
                this.transform.GetChild(0).GetChild(0).localRotation = this.transform.GetChild(0).GetChild(0).localRotation* Quaternion.Euler(childRotationRate);
            }
            // E to rotate sphere 4 around Z-Axis
            if (Input.GetKey(KeyCode.E))
            {
                rot -= 20.0f * Time.deltaTime;
                childRotationRate = new Vector3(0,0,rot);
                this.transform.GetChild(0).GetChild(0).localRotation = this.transform.GetChild(0).GetChild(0).localRotation* Quaternion.Euler(childRotationRate);
            }


            }  else if (key == "h") { //if the current object is a big finger

            // H to rotate big fingers 20 degrees around X-axis
            if (Input.GetKey(KeyCode.H))
            {
                rot += 20.0f * Time.deltaTime;
                rotationRate = new Vector3(rot,0,0);
                transform.localRotation = transform.localRotation * Quaternion.Euler(rotationRate);
            }

            } else { // if its a small finger 

            // H to rotate small fingers 50 degrees around X-axis
            if (Input.GetKey(KeyCode.H))
            {
                rot += 50.0f * Time.deltaTime;
                rotationRate = new Vector3(rot,0,0);
                transform.localRotation = transform.localRotation * Quaternion.Euler(rotationRate);
            }
            }


        } else {


            // if the current gameobject is sphere 2:
            if (key == "t") {

                 // T to rotate sphere 2 around Y-axis
            if (Input.GetKey(KeyCode.T))
            {
                rot += 30.0f * Time.deltaTime;
                rotationRate = new Vector3(0,rot,0);
                transform.localRotation = transform.localRotation * Quaternion.Euler(rotationRate);
            }

            // R to raise sphere 2 
            if (Input.GetKey(KeyCode.R))
            {
                translationRate = new Vector3(0, 0.5f, 0);
                transform.Translate(translationRate * Time.deltaTime);
            }
            // U to rotate sphere 2 around Z-Axis ( and rotate sphere 4 in reverse to stay parralel )
            if (Input.GetKey(KeyCode.U))
            {
                rot += 20.0f * Time.deltaTime;
                rotationRate = new Vector3(0,0,rot);
                transform.localRotation = transform.localRotation * Quaternion.Euler(rotationRate);
                //now rotate sphere 4 in reverse
                otherRot -= 20.0f * Time.deltaTime;
                childRotationRate = new Vector3(0,0,otherRot);
                this.transform.GetChild(0).GetChild(0).localRotation = this.transform.GetChild(0).GetChild(0).localRotation* Quaternion.Euler(childRotationRate);
            }
            // E to rotate sphere 4 around Z-Axis
            if (Input.GetKey(KeyCode.E))
            {
                rot += 20.0f * Time.deltaTime;
                childRotationRate = new Vector3(0,0,rot);
                this.transform.GetChild(0).GetChild(0).localRotation = this.transform.GetChild(0).GetChild(0).localRotation* Quaternion.Euler(childRotationRate);
            }

            }  else if (key == "h") { //if the current object is a big finger

            // H to rotate big fingers around X-axis
            if (Input.GetKey(KeyCode.H))
            {
                rot -= 20.0f * Time.deltaTime;
                rotationRate = new Vector3(rot,0,0);
                transform.localRotation = transform.localRotation * Quaternion.Euler(rotationRate);
            }
            
        } else { // if its a small finger 

            // H to rotate small fingers 50 degrees around X-axis
            if (Input.GetKey(KeyCode.H))
            {
                rot -= 50.0f * Time.deltaTime;
                rotationRate = new Vector3(rot,0,0);
                transform.localRotation = transform.localRotation * Quaternion.Euler(rotationRate);
            }
            }



        } //end else shift is pressed

       
    }
}
