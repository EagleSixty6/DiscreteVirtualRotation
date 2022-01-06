using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlaceObjects : MonoBehaviour
{
    //Prefabs
    public GameObject balloonPref;
    public GameObject carPref;
    public GameObject cubePref;
    public GameObject cylinderPref;
    public GameObject pottedTreePref;
    public GameObject pyramidePref;
    public GameObject ringPref;
    public GameObject spherePref;
    public GameObject starPref;

    public GameObject laptopPref;
    public GameObject pirateShipPref;
    public GameObject toasterPref;

    public GameObject tablePref;
    public GameObject glassTablePref;
    public GameObject helpTablePref;
    public List<GameObject> helpTables;
    public GameObject chairPref;
    public GameObject shelfPref;

    public GameObject doorPref;
    public GameObject woodenGuitarPref;
    public GameObject blackGuitarPref;
    public GameObject grillPref;
    public GameObject lampPref;
    public GameObject pianoPref;
    public GameObject pingPongTablePref;
    public GameObject wellPref;

    public List<GameObject> placementPrefabs;

    //Lists of objects
    public List<GameObject> allObjects;

    public List<GameObject> smallObjects;

    public List<GameObject> tables;
    public List<GameObject> chairs;

    public List<GameObject> shelfs;

    public List<int> balloonsAngles;
    public List<int> obj3;
    public List<int> obj2;
    public List<int> obj1;
    public List<List<int>> objectsAngles;

    // Materials/colours
    public Material[] mats;
    public List<Material> materials;

    //Radii
    public float[] radii;

    // Randomizer
    public System.Random rnd;

    // Seeds
    public int[] seeds;

    //public Main mainScript;

    // Start is called before the first frame update
    void Start()
    {
        //mainScript = GetComponent<Main>();

        materials = new List<Material>();
        materials.AddRange(mats);

        allObjects = new List<GameObject>();
        smallObjects = new List<GameObject>();
        helpTables = new List<GameObject>();
        placementPrefabs = new List<GameObject>() { carPref, cubePref, cylinderPref, pottedTreePref, pyramidePref, ringPref, spherePref, starPref, pirateShipPref, toasterPref };
        tables = new List<GameObject>();
        chairs = new List<GameObject>();
        shelfs = new List<GameObject>();

        balloonsAngles = new List<int>() { -1 };
        obj3 = new List<int>() { -1 };
        obj2 = new List<int>() { -1 };
        obj1 = new List<int>() { -1 };
        objectsAngles = new List<List<int>>() { obj1, obj2, obj3 };

        radii = new float[] { 3.5f, 5.25f, 7f };

        seeds = new int[] { 72, 152, 346, 413, 421 };

        rnd = new System.Random(0);

        GetComponent<Task2>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaceAllObjects()
    {
        PlaceUniqueObjects();
        PlaceTablesAndChairs();
        PlaceShelfs();
        PlaceSmallObjects();
        FillTheTables();
        FillTheShelfs();
    }

    public void GenerateAllObjects(int s)
    {
        rnd = new System.Random(s);

        GenerateObject(pingPongTablePref);
        GenerateObject(wellPref);
        GenerateObject(pianoPref);
        GenerateObject(doorPref);

        GenerateObject(lampPref);
        GenerateObject(grillPref);
        GenerateObject(woodenGuitarPref);
        GenerateObject(blackGuitarPref);

        GenerateObjects(3, 5, tablePref, tables);
        GenerateObjects(tables.Count, tables.Count * 3, chairPref, chairs);

        GenerateObjects(3, 5, shelfPref, shelfs);

        GenerateObjects(4, 5, balloonPref, smallObjects);
        GenerateObjects(4, 5, cubePref, smallObjects);
        GenerateObjects(4, 5, cylinderPref, smallObjects);
        GenerateObjects(4, 5, pottedTreePref, smallObjects);
        GenerateObjects(4, 5, pyramidePref, smallObjects);
        GenerateObjects(4, 5, ringPref, smallObjects);
        GenerateObjects(4, 5, spherePref, smallObjects);
        GenerateObjects(4, 5, starPref, smallObjects);
    }

    public void DeleteAllObjects()
    {
        foreach (GameObject o in allObjects)
        {
            Destroy(o);
        }
        allObjects.Clear();
        tables.Clear();
        helpTables.Clear();
        chairs.Clear();
        shelfs.Clear();
        smallObjects.Clear();

        obj3.Clear();
        obj3.Add(-1);
        obj2.Clear();
        obj2.Add(-1);
        obj1.Clear();
        obj1.Add(-1);

        objectsAngles.Clear();
        objectsAngles.Add(obj1);
        objectsAngles.Add(obj2);
        objectsAngles.Add(obj3);

        balloonsAngles.Clear();
        balloonsAngles.Add(-1);
    }

    private void GenerateObjects(int min, int max, GameObject pref, List<GameObject> list)
    {
        for (int i = 0; i < rnd.Next(min, max + 1); i++)
        {
            GameObject temp;
            if (i == 0 && pref.Equals(tablePref))
            {
                temp = Instantiate(glassTablePref, glassTablePref.transform.position, Quaternion.identity);
                temp.name = "Glass Table";
            }

            else
            {
                temp = Instantiate(pref, pref.transform.position, Quaternion.identity);
                temp.name = temp.name.Split('(')[0];
                if (!pref.Equals(chairPref))
                {
                    int matIndex = rnd.Next(0, materials.Count);
                    SetColor(temp, materials[matIndex]);
                    temp.name = materials[matIndex].name.Split(' ')[0] + " " + temp.name;
                    materials.RemoveAt(matIndex);
                }
            }
            list.Add(temp);
            allObjects.Add(temp);
        }
        materials.Clear();
        materials.AddRange(mats);
    }

    private void GenerateObject(GameObject pref)
    {
        GameObject temp = Instantiate(pref, pref.transform.position, Quaternion.identity);
        temp.name = temp.name.Split('(')[0];

        allObjects.Add(temp);
    }

    private void PlaceUniqueObjects()
    {
        for (int i = 0; i < 8; i++)
        {
            List<int> angles = new List<int>();
            bool placingPossible = false;
            int angle = 0;
            int range = 14;

            allObjects[i].transform.position += allObjects[i].transform.forward * radii[2 - (i / 4)];
            if (i == 1) { range = 9; }
            else if (i == 2) { range = 10; }
            else if (i == 3) { range = 5; }
            else if (i == 4) { range = 3; }
            else if (i == 5) { range = 6; }
            else if (i != 0) { range = 4; }

            while (!placingPossible)
            {
                placingPossible = true;
                angle = rnd.Next(0, 360);

                for (int j = angle - range; j < angle + range; j++)
                {
                    angles.Add((j + 360) % 360);
                }

                for (int j = 0; j < angles.Count; j++)
                {
                    if (objectsAngles[2].Contains(angles[j]))
                    {
                        placingPossible = false;
                        angles.Clear();
                        break;
                    }
                }
            }

            objectsAngles[2 - (i / 4)].AddRange(angles);

            if (i == 2) { allObjects[i].transform.RotateAround(allObjects[i].transform.position, Vector3.up, 220); }
            else if (i > 4) { allObjects[i].transform.RotateAround(allObjects[i].transform.position, Vector3.up, 180); }

            allObjects[i].transform.RotateAround(Vector3.zero, Vector3.up, angle);

            objectsAngles[2 - (i / 4)].Add(-1);

        }
    }

    private void PlaceTablesAndChairs()
    {
        int chairCount = chairs.Count;
        int tableCount = tables.Count;

        int[] chairsPerTable = new int[tableCount];

        while (chairCount > 0)
        {
            int index = rnd.Next(0, tableCount);
            if (chairsPerTable[index] < 3)
            {
                chairsPerTable[index]++;
                chairCount--;
            }
        }

        int chairIndex = 0;
        tableCount--;

        while (tableCount >= 0)
        {
            int radius = rnd.Next(1, 3);

            GameObject helpTable = Instantiate(helpTablePref, helpTablePref.transform.position, Quaternion.identity);
            tables[tableCount].transform.position += tables[tableCount].transform.forward * radii[radius];
            helpTable.transform.position += tables[tableCount].transform.forward * radii[radius];
            helpTable.name += tableCount;

            int ang = 0;
            List<int> angles = new List<int>();
            bool placingPossible = false;

            while (!placingPossible)
            {
                placingPossible = true;
                ang = rnd.Next(0, 360);

                for (int i = ang - (17 - 4 * (radius - 1)); i < ang + (17 - 4 * (radius - 1)); i++)
                {
                    angles.Add((i + 360) % 360);
                }

                for (int i = 0; i < angles.Count; i++)
                {
                    if (objectsAngles[radius].Contains(angles[i]))
                    {
                        placingPossible = false;
                        angles.Clear();
                        break;
                    }
                }
            }

            objectsAngles[radius].AddRange(angles);

            if (!tables[tableCount].name.Contains("Glass"))
            {
                tables[tableCount].transform.rotation = Quaternion.Euler(-90, 90, 0);
            }

            tables[tableCount].transform.RotateAround(Vector3.zero, Vector3.up, ang);
            helpTable.transform.RotateAround(Vector3.zero, Vector3.up, ang);
            helpTables.Add(helpTable);

            objectsAngles[radius].Add(-1);

            bool[] free = { true, true, true };

            while (chairsPerTable[tableCount] > 0)
            {
                int index = rnd.Next(0, 3);
                if (free[index])
                {
                    if (index == 0)
                    {
                        chairs[chairIndex].transform.position = new Vector3(-1.1f, 0.427f, 0);
                        chairs[chairIndex].transform.rotation = Quaternion.Euler(0, 0, 0);
                        chairs[chairIndex].transform.position += Vector3.forward * radii[radius];
                        chairs[chairIndex].transform.RotateAround(Vector3.zero, Vector3.up, ang);

                        int matIndex = rnd.Next(0, materials.Count);
                        SetColor(chairs[chairIndex], materials[matIndex]);
                        chairs[chairIndex].name = materials[matIndex].name.Split(' ')[0] + " " + chairs[chairIndex].name + " at the " + tables[tableCount].name;
                        materials.RemoveAt(matIndex);

                        free[index] = false;
                    }
                    else if (index == 1)
                    {
                        chairs[chairIndex].transform.position = new Vector3(0, 0.427f, 0.65f);
                        chairs[chairIndex].transform.rotation = Quaternion.Euler(0, 90, 0);
                        chairs[chairIndex].transform.position += Vector3.forward * radii[radius];
                        chairs[chairIndex].transform.RotateAround(Vector3.zero, Vector3.up, ang);

                        int matIndex = rnd.Next(0, materials.Count);
                        SetColor(chairs[chairIndex], materials[matIndex]);
                        chairs[chairIndex].name = materials[matIndex].name.Split(' ')[0] + " " + chairs[chairIndex].name + " at the " + tables[tableCount].name;
                        materials.RemoveAt(matIndex);

                        free[index] = false;
                    }
                    else
                    {
                        chairs[chairIndex].transform.position = new Vector3(1.1f, 0.427f, 0);
                        chairs[chairIndex].transform.rotation = Quaternion.Euler(0, 180, 0);
                        chairs[chairIndex].transform.position += Vector3.forward * radii[radius];
                        chairs[chairIndex].transform.RotateAround(Vector3.zero, Vector3.up, ang);

                        int matIndex = rnd.Next(0, materials.Count);
                        SetColor(chairs[chairIndex], materials[matIndex]);
                        chairs[chairIndex].name = materials[matIndex].name.Split(' ')[0] + " " + chairs[chairIndex].name + " at the " + tables[tableCount].name;
                        materials.RemoveAt(matIndex);

                        free[index] = false;
                    }
                    chairIndex++;
                    chairsPerTable[tableCount]--;
                }
            }
            tableCount--;
            materials.Clear();
            materials.AddRange(mats);
        }

    }

    private void PlaceShelfs()
    {
        int shelfCount = shelfs.Count - 1;

        while (shelfCount >= 0)
        {
            int ang = 0;
            List<int> angles = new List<int>();
            bool placingPossible = false;
            int matIndex = rnd.Next(0, materials.Count);

            shelfs[shelfCount].transform.position += shelfs[shelfCount].transform.forward * radii[2];

            while (!placingPossible)
            {
                placingPossible = true;
                ang = rnd.Next(0, 360);
                for (int i = ang - 8; i < ang + 8; i++)
                {
                    angles.Add((i + 360) % 360);
                }

                for (int i = 0; i < angles.Count; i++)
                {
                    if (objectsAngles[2].Contains(angles[i]))
                    {
                        placingPossible = false;
                        angles.Clear();
                        break;
                    }
                }
            }

            objectsAngles[2].AddRange(angles);

            shelfs[shelfCount].transform.RotateAround(Vector3.zero, Vector3.up, ang);

            objectsAngles[2].Add(-1);

            List<GameObject> children = new List<GameObject>();
            for (int i = 0; i < shelfs[shelfCount].transform.childCount; i++)
            {
                children.Add(shelfs[shelfCount].transform.GetChild(i).gameObject);
            }
            foreach (GameObject child in children)
            {
                if (child.name.StartsWith("B"))
                {
                    child.GetComponent<MeshRenderer>().material = materials[matIndex];
                }
            }

            materials.RemoveAt(matIndex);

            shelfCount--;
        }
        materials.Clear();
        materials.AddRange(mats);
    }

    private void PlaceSmallObjects()
    {
        int oCount = smallObjects.Count - 1;

        List<int> free0 = new List<int>();
        List<int> free1 = new List<int>();
        List<int> free2 = new List<int>();
        List<List<int>> free = new List<List<int>>() { free0, free1, free2 };

        for (int i = 0; i < 360; i++)
        {
            if (!objectsAngles[0].Contains(i))
            {
                free[0].Add(i);
            }
            if (!objectsAngles[1].Contains(i))
            {
                free[1].Add(i);
            }
            if (!objectsAngles[2].Contains(i))
            {
                free[2].Add(i);
            }
        }

        while (oCount >= 0)
        {
            int ang = 0;
            List<int> angles = new List<int>();
            bool placingPossible = false;

            List<int> radiusList = new List<int>() { 0, 1, 2 };

            //int radius = rnd.Next(0,3);
            int radius = radiusList[rnd.Next(0, radiusList.Count)];
            radiusList.Remove(radius);

            if (smallObjects[oCount].name.Contains("Balloon"))
            {
                radius = 1;

                while (!placingPossible)
                {
                    placingPossible = true;
                    ang = rnd.Next(0, 360);
                    for (int i = ang - 5; i < ang + 5; i++)
                    {
                        angles.Add((i + 360) % 360);
                    }

                    for (int i = 0; i < angles.Count; i++)
                    {
                        if (balloonsAngles.Contains(angles[i]))
                        {
                            placingPossible = false;
                            angles.Clear();
                            break;
                        }
                    }
                }

                balloonsAngles.AddRange(angles);

                smallObjects[oCount].transform.position += smallObjects[oCount].transform.forward * radii[radius];

                smallObjects[oCount].transform.RotateAround(Vector3.zero, Vector3.up, ang);

                balloonsAngles.Add(-1);
            }
            else
            {
                while (!placingPossible)
                {
                    if (radiusList.Count == 0)
                    {
                        break;
                    }
                    if (free[radius].Count == 0)
                    {
                        radius = radiusList[rnd.Next(0, radiusList.Count)];
                        radiusList.Remove(radius);
                        continue;
                    }
                    placingPossible = true;
                    ang = free[radius][rnd.Next(0, free[radius].Count)];
                    int range = SmallObjectRange(smallObjects[oCount].name, radius);

                    for (int i = ang - range; i < ang + range; i++)
                    {
                        angles.Add((i + 360) % 360);
                    }

                    for (int i = 0; i < angles.Count; i++)
                    {
                        if (objectsAngles[radius].Contains(angles[i]))
                        {
                            free[radius].Remove(ang);
                            placingPossible = false;
                            angles.Clear();
                            break;
                        }
                    }
                }

                if (!placingPossible)
                {
                    Destroy(smallObjects[oCount]);
                    allObjects.Remove(smallObjects[oCount]);
                    oCount--;
                    continue;
                }

                objectsAngles[radius].AddRange(angles);

                smallObjects[oCount].transform.position += smallObjects[oCount].transform.forward * radii[radius];

                smallObjects[oCount].transform.RotateAround(Vector3.zero, Vector3.up, ang);

                if (radius == 0 && (smallObjects[oCount].name.Contains("Tree") || smallObjects[oCount].name.Contains("Pyramid")))
                {
                    smallObjects[oCount].transform.localScale *= 0.6f;
                }

                objectsAngles[radius].Add(-1);

                smallObjects[oCount].name += " on the floor";
            }

            oCount--;
        }

    }

    private void SetColor(GameObject o, Material m)
    {
        string oName = o.name;
        List<GameObject> children = new List<GameObject>();
        for (int i = 0; i < o.transform.childCount; i++)
        {
            children.Add(o.transform.GetChild(i).gameObject);
        }

        if (oName.Contains("Car"))
        {
            foreach (GameObject child in children)
            {
                if (child.name.Equals("Top"))
                {
                    child.GetComponent<MeshRenderer>().material = m;
                }
            }
            o.GetComponent<MeshRenderer>().material = m;
        }
        else if (oName.StartsWith("Po"))
        {
            foreach (GameObject child in children)
            {
                if (child.name.Equals("Pot"))
                {
                    child.GetComponent<MeshRenderer>().material = m;
                }
            }
        }
        else if (oName.StartsWith("Sh"))
        {
            foreach (GameObject child in children)
            {
                if (child.name.Equals("Shelf"))
                {
                    child.GetComponent<MeshRenderer>().material = m;
                }
            }
        }
        else if (oName.StartsWith("Py"))
        {
            foreach (GameObject child in children)
            {
                child.GetComponent<MeshRenderer>().material = m;
            }
            o.GetComponent<MeshRenderer>().material = m;
        }
        else if (oName.StartsWith("B") || oName.StartsWith("Ch"))
        {
            foreach (GameObject child in children)
            {
                child.GetComponent<MeshRenderer>().material = m;
            }
        }
        else
        {
            o.GetComponent<MeshRenderer>().material = m;
        }
    }

    private void FillTheTables()
    {
        int tableCount = tables.Count - 1;
        bool laptopPlaced = false;
        int laptopTable = rnd.Next(0, tableCount + 1);
        List<GameObject> tableObjects = new List<GameObject>();

        while (tableCount >= 0)
        {
            List<float> free = new List<float> { -0.75f, 0, 0.75f };

            int itemCount = rnd.Next(1, 4);
            tableObjects.Clear();
            tableObjects.AddRange(placementPrefabs);

            while (itemCount > 0)
            {
                int freeIndex = rnd.Next(0, free.Count);

                GameObject temp;

                Vector3 positionOnTable = helpTables[tables.Count - 1 - tableCount].transform.position;
                positionOnTable += free[freeIndex] * helpTables[tables.Count - 1 - tableCount].transform.right;
                positionOnTable += (float)(rnd.NextDouble() * 0.6 - 0.3) * free[freeIndex] * helpTables[tables.Count - 1 - tableCount].transform.forward;

                if (tableCount == laptopTable && !laptopPlaced)
                {
                    positionOnTable.y = 1.05f;
                    temp = Instantiate(laptopPref, positionOnTable, helpTables[tables.Count - 1 - tableCount].transform.rotation);
                    temp.transform.RotateAround(positionOnTable, Vector3.up, 270);
                    temp.name = "Laptop on the " + tables[tableCount].name;
                    laptopPlaced = true;
                }
                else
                {
                    int objectIndex = rnd.Next(0, tableObjects.Count);
                    positionOnTable.y = ObjectTableHeight(tableObjects[objectIndex]);
                    temp = Instantiate(tableObjects[objectIndex], positionOnTable, helpTables[tables.Count - 1 - tableCount].transform.rotation);

                    if (tableObjects[objectIndex].Equals(pyramidePref) || tableObjects[objectIndex].Equals(cylinderPref))
                    {
                        temp.transform.localScale = temp.transform.localScale * 0.5f;
                    }

                    if (!tableObjects[objectIndex].Equals(pirateShipPref) && !tableObjects[objectIndex].Equals(toasterPref))
                    {
                        int matIndex = rnd.Next(0, materials.Count);
                        SetColor(temp, materials[matIndex]);
                        temp.name = materials[matIndex].name.Split(' ')[0] + " " + temp.name.Split('(')[0] + " on the " + tables[tableCount].name;
                    }
                    else
                    {
                        temp.name = temp.name.Split('(')[0] + " on the " + tables[tableCount].name;
                        if (tableObjects[objectIndex].Equals(pirateShipPref))
                        {
                            temp.transform.RotateAround(temp.transform.position, Vector3.up, 130);
                        }
                        else
                        {
                            temp.transform.RotateAround(temp.transform.position, Vector3.up, 90);
                        }
                    }

                    tableObjects.RemoveAt(objectIndex);
                }

                allObjects.Add(temp);

                helpTables[tableCount].SetActive(false);

                free.RemoveAt(freeIndex);
                itemCount--;
            }
            tableCount--;
        }
    }

    private void FillTheShelfs()
    {
        int shelfCount = shelfs.Count - 1;

        while (shelfCount >= 0)
        {
            bool[,] free = new bool[,] { { true, true, true }, { true, true, true }, { true, true, true } };

            int itemCount = rnd.Next(3, 8);
            List<GameObject> shelfObjects = new List<GameObject>();
            shelfObjects.AddRange(placementPrefabs);

            while (itemCount > 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (free[i, j] && rnd.NextDouble() < 0.5 && itemCount > 0)
                        {
                            free[i, j] = false;
                            itemCount--;
                        }
                    }
                }
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (!free[i, j])
                    {
                        int objectIndex = rnd.Next(0, shelfObjects.Count - 1);

                        Vector3 positionInShelf = shelfs[shelfCount].transform.position;

                        float xPos = -0.52f;
                        if (j == 1)
                        {
                            xPos = 0;
                        }
                        else if (j == 2)
                        {
                            xPos = 0.52f;
                        }

                        positionInShelf += xPos * shelfs[shelfCount].transform.right;
                        positionInShelf.y = OjectShelfHeight(shelfObjects[objectIndex], i);

                        GameObject temp = Instantiate(shelfObjects[objectIndex], positionInShelf, shelfs[shelfCount].transform.rotation);
                        temp.transform.localScale = temp.transform.localScale * ObjectShelfScale(shelfObjects[objectIndex]);


                        if (!shelfObjects[objectIndex].Equals(pirateShipPref) && !shelfObjects[objectIndex].Equals(toasterPref))
                        {
                            int matIndex = rnd.Next(0, materials.Count);
                            SetColor(temp, materials[matIndex]);
                            temp.name = materials[matIndex].name.Split(' ')[0] + " " + temp.name.Split('(')[0] + " in the " + shelfs[shelfCount].name;
                        }
                        else
                        {
                            temp.name = temp.name.Split('(')[0] + " in the " + shelfs[shelfCount].name;
                            if (shelfObjects[objectIndex].Equals(pirateShipPref))
                            {
                                temp.transform.RotateAround(temp.transform.position, Vector3.up, 130);
                            }
                            else
                            {
                                temp.transform.RotateAround(temp.transform.position, Vector3.up, 90);
                            }
                        }
                        shelfObjects.RemoveAt(objectIndex);


                        allObjects.Add(temp);
                        itemCount--;
                    }
                }
            }
            shelfCount--;
        }
    }

    private int SmallObjectRange(string name, int r)
    {
        if (name.Contains("Cube") || name.Contains("Cyl") || name.Contains("Pot") || name.Contains("Sph") || name.Contains("Star"))
        {
            return 2 * (3 + r);
        }
        else if (name.Contains("Py"))
        {
            return 2 * (int)(5 + 2.5f * r);
        }

        return 2 * (int)(3 + 1.5f * r);
    }

    private float ObjectTableHeight(GameObject pref)
    {
        if (pref.Equals(carPref))
        {
            return 1.208f;
        }
        else if (pref.Equals(cubePref) || pref.Equals(spherePref))
        {
            return 1.308f;
        }
        else if (pref.Equals(cylinderPref))
        {
            return 1.262f;
        }
        else if (pref.Equals(pottedTreePref))
        {
            return 1.058f;
        }
        else if (pref.Equals(pyramidePref))
        {
            return 1.108f;
        }
        else if (pref.Equals(ringPref))
        {
            return 1.3705f;
        }
        else if (pref.Equals(starPref))
        {
            return 1.298f;
        }
        else
        {
            return 1.05f;
        }
    }

    private float ObjectShelfScale(GameObject pref)
    {
        if (pref.Equals(pirateShipPref) || pref.Equals(spherePref) || pref.Equals(starPref))
        {
            return 0.8f;
        }
        else if (pref.Equals(ringPref))
        {
            return 0.6f;
        }
        else if (pref.Equals(carPref) || pref.Equals(cubePref) || pref.Equals(cylinderPref))
        {
            return 0.5f;
        }
        else if (pref.Equals(pottedTreePref))
        {
            return 0.4f;
        }
        else if (pref.Equals(pyramidePref))
        {
            return 0.3f;
        }
        return 1;
    }

    private float OjectShelfHeight(GameObject pref, int level)
    {
        if (pref.Equals(carPref))
        {
            if (level == 0)
            {
                return 0.698f;
            }
            else if (level == 1)
            {
                return 1.262f;
            }
            else
            {
                return 1.824f;
            }
        }
        else if (pref.Equals(cubePref))
        {
            if (level == 0)
            {
                return 0.740f;
            }
            else if (level == 1)
            {
                return 1.313f;
            }
            else
            {
                return 1.876f;
            }
        }
        else if (pref.Equals(cylinderPref))
        {
            if (level == 0)
            {
                return 0.822f;
            }
            else if (level == 1)
            {
                return 1.386f;
            }
            else
            {
                return 1.949f;
            }
        }
        else if (pref.Equals(pottedTreePref))
        {
            if (level == 0)
            {
                return 0.621f;
            }
            else if (level == 1)
            {
                return 1.183f;
            }
            else
            {
                return 1.748f;
            }
        }
        else if (pref.Equals(pyramidePref))
        {
            if (level == 0)
            {
                return 0.635f;
            }
            else if (level == 1)
            {
                return 1.2f;
            }
            else
            {
                return 1.762f;
            }
        }
        else if (pref.Equals(ringPref))
        {
            if (level == 0)
            {
                return 0.808f;
            }
            else if (level == 1)
            {
                return 1.372f;
            }
            else
            {
                return 1.934f;
            }
        }
        else if (pref.Equals(spherePref))
        {
            if (level == 0)
            {
                return 0.82f;
            }
            else if (level == 1)
            {
                return 1.386f;
            }
            else
            {
                return 1.947f;
            }
        }
        else if (pref.Equals(starPref))
        {
            if (level == 0)
            {
                return 0.814f;
            }
            else if (level == 1)
            {
                return 1.38f;
            }
            else
            {
                return 1.943f;
            }
        }
        else if (pref.Equals(toasterPref))
        {
            if (level == 0)
            {
                return 0.616f;
            }
            else if (level == 1)
            {
                return 1.18f;
            }
            else
            {
                return 1.742f;
            }
        }
        else
        {
            if (level == 0)
            {
                return 0.62f;
            }
            else if (level == 1)
            {
                return 1.186f;
            }
            else
            {
                return 1.747f;
            }
        }
    }

    public void NewEnvironment(int i)
    {
        DeleteAllObjects();
        if (i < 5)
        {
            GenerateAllObjects(seeds[i]);
        }
        else
        {
            GenerateAllObjects(i);
        }
        PlaceAllObjects();
    }

    public void SortByArray(int[] array)
    {
        int[] copy = new int[5];
        for (int i = 0; i < 5; i++)
        {
            copy[i] = seeds[array[i]];
        }
        for (int i = 0; i < 5; i++)
        {
            seeds[i] = copy[i];
        }
    }
}
