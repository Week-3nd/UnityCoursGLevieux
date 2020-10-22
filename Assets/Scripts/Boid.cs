using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public float zoneRepulsion = 5;
    public float zoneAlignement = 9;
    public float zoneAttraction = 50;

    public float forceRepulsion = 15;
    public float forceAlignement = 3;
    public float forceAttraction = 20;

    public Vector3 target = new Vector3();
    public float forceTarget = 20;
    public bool goToTarget = false;

    public Vector3 velocity = new Vector3();
    public float maxSpeed = 20;
    public float minSpeed = 12;

    public bool drawGizmos = true;
    public bool drawLines = false;

    // Update is called once per frame
    void Update()
    {
        Vector3 sumForces = new Vector3();
        Color colorDebugForce = Color.black;
        float nbForcesApplied = 0;

        //Accède a tous les autres boids grace à la liste read only du manager
        foreach (Boid otherBoid in BoidManager.sharedInstance.roBoids)
        {
            Vector3 vecToOtherBoid = otherBoid.transform.position - transform.position; //calcule distance

            Vector3 forceToApply = new Vector3(); // met en mémoire une force à appliquer /!\ pas encore calculée /!\

            //Si on doit prendre en compte cet autre boid (plus grande zone de perception)
            if (vecToOtherBoid.sqrMagnitude < zoneAttraction * zoneAttraction)
            {
                //Si on est entre attraction et alignement
                if (vecToOtherBoid.sqrMagnitude > zoneAlignement * zoneAlignement)
                {
                    //On est dans la zone d'attraction uniquement
                    //forceToApply = vecToOtherBoid.normalized * forceAttraction; //calcule la force à appliquer
                    float distToOtherBoid = vecToOtherBoid.magnitude;
                    float normalizedDistanceToNextZone = ((distToOtherBoid - zoneAlignement) / (zoneAttraction - zoneAlignement));
                    float boostForce = (4 * normalizedDistanceToNextZone); // ces deux lignes donnent un facteur de boost selon la proximité du boid (le facteur 4 est arbitraire)
                    forceToApply = vecToOtherBoid.normalized * forceAttraction * boostForce; //calcule une deuxième fois la force?? ici force proportionnelle à la distance au boid
                    colorDebugForce += Color.green; //attraction = vert
                }
                else
                {
                    //On est dans alignement, mais est on hors de répulsion ?
                    if (vecToOtherBoid.sqrMagnitude > zoneRepulsion * zoneRepulsion)
                    {
                        //On est dans la zone d'alignement uniquement
                        forceToApply = otherBoid.velocity.normalized * forceAlignement; //otherBoid.velocity.normalized c'est sa direction normalisée
                        colorDebugForce += Color.blue; //alignement = bleu
                    }
                    else
                    {
                        //On est dans la zone de repulsion
                        float distToOtherBoid = vecToOtherBoid.magnitude;
                        float normalizedDistanceToPreviousZone = 1 - (distToOtherBoid / zoneRepulsion); //contrairelent à la condition similaire pour la zone d'attraction, on utilise 1 car c'est la zone la plus proche du boid
                        float boostForce = (4 * normalizedDistanceToPreviousZone);
                        forceToApply = vecToOtherBoid.normalized * -1 * (forceRepulsion * boostForce);
                        colorDebugForce += Color.red; // répulsion = rouge

                    }
                }

                sumForces += forceToApply;
                nbForcesApplied++;
            }
        }

        //On fait la moyenne des forces, ce qui nous rend indépendant du nombre de boids
        sumForces /= nbForcesApplied;
        
        //Si on a une target, on l'ajoute
        if (goToTarget)
        {
            Vector3 vecToTarget = target - transform.position;
            if (vecToTarget.sqrMagnitude < 1)
                goToTarget = false;
            else
            {
                Vector3 forceToTarget = vecToTarget.normalized * forceTarget;
                sumForces += forceToTarget;
                colorDebugForce += Color.magenta;
                nbForcesApplied++;
                if (drawLines)
                    Debug.DrawLine(transform.position, target, Color.magenta);
            }
        }

        //On freine
        velocity += -velocity * 10 * Vector3.Angle(sumForces, velocity) / 180.0f * Time.deltaTime;

        //on applique les forces
        velocity += sumForces * Time.deltaTime;

        //On limite la vitesse
        if (velocity.sqrMagnitude > maxSpeed * maxSpeed)
            velocity = velocity.normalized * maxSpeed;
        if (velocity.sqrMagnitude < minSpeed * minSpeed)
            velocity = velocity.normalized * minSpeed;

        //On regarde dans la bonne direction        
        if (velocity.sqrMagnitude > 0)
            transform.LookAt(transform.position + velocity);

        //Debug
        if (drawLines)
            Debug.DrawLine(transform.position, transform.position + velocity, Color.blue);

        //Deplacement du boid
        transform.position += velocity * Time.deltaTime;
    }

    void OnDrawGizmosSelected()
    {
        if (drawGizmos)
        {
            // Répulsion
            Gizmos.color = new Color(1, 0, 0, 1.0f);
            Gizmos.DrawWireSphere(transform.position, zoneRepulsion);
            // Alignement
            Gizmos.color = new Color(0, 1, 0, 1.0f);
            Gizmos.DrawWireSphere(transform.position, zoneAlignement);
            // Attraction
            Gizmos.color = new Color(0, 0, 1, 1.0f);
            Gizmos.DrawWireSphere(transform.position, zoneAttraction);
        }
    }
}