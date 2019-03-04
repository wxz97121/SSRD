using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SrdTap : MonoBehaviour {
	[SerializeField] private GameObject shape;
	[SerializeField] private int type = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			NewShape (type);
		}
	}
	//addkeyfram (time_start, time_end, pivot_x, pivot_y, pointlist, size, radius, arc, rotationlist, color, alpha, width, progress_start, progress_end, linear, outline, connect, tweenmode )
	public void NewShape (int type){
		//float x = Input.GetAxis ("Mouse X");
	    //float y = Input.GetAxis ("Mouse Y");
		//0-7为测试动画，不使用
		//8会卡顿
		float midx=0f;
		if (type == 0) {
			Vector3[] point1 = Frame.getRandomPoint (5, 8f, 6f);
			Vector3[] point2 = Frame.getRandomPoint (5, 8f, 6f);
			Vector3 pos = new Vector3 (midx, 0, 0);
			var inst = Instantiate (shape, pos, transform.rotation);
			Shape s = inst.GetComponent<Shape> ();
			Color col = Color.grey;
			s.addKeyframe (0, 60, 0, 0, point1, 1f, 1f, 0, new Rotation[0], col, 1.0f, 0.1f, 0, 1.0f, true, false, false, Tweenmode.Expo_easeOut);
			s.addKeyframe (60, 90, 0, 0, point2, 1f, 1f, 0, new Rotation[0], col, 1.0f, 0.1f, 0, 1.0f, true, false, false, Tweenmode.Linear);
			s.addKeyframe (90, 95, 0, 0, point2, 1f, 1f, 2 * Mathf.PI, new Rotation[0], col, 0, 0.1f, 0, 1.0f, true, false, false, Tweenmode.Linear);
			s.animOn ();
		}
		if (type == 1) {
			Vector3[] point1 = Frame.getRandomPoint (5, 4f, 3f);
			Vector3[] point2 = Frame.getRandomPoint (5, 4f, 3f);
			Vector3 pos = new Vector3 (midx, 0, 0);
			var inst = Instantiate (shape, pos, transform.rotation);
			Shape s = inst.GetComponent<Shape> ();
            Color col = new Color(128,0,0,128);
            float widthrand = Random.Range (0.05f, 0.3f);
			s.addKeyframe (0, 60, 0, 0, point1, 3f, 1f, 0, new Rotation[0], col, 1.0f, widthrand, 0, 0, true, true, false, Tweenmode.Expo_easeOut);
			s.addKeyframe (60, 90, 0, 0, point1, 3f, 1f, 2 * Mathf.PI, new Rotation[0], col, 0.7f, widthrand, 0, 1.0f, true, true, false, Tweenmode.Linear);
			s.addKeyframe (90, 95, 0, 0, point1, 3f, 1f, 2 * Mathf.PI, new Rotation[0], col, 0, widthrand, 0, 1.0f, true, true, false, Tweenmode.Linear);
			s.animOn ();
		}
		if (type == 2) {
			Vector3[] point1 = Frame.getPolygonPoint (6 ,1f);
			var inst = Instantiate (shape, transform.position, transform.rotation);

			Shape s = inst.GetComponent<Shape> ();
			s.addKeyframe (0, 60, 0, 0, point1, 1f, 1f, 0, new Rotation[0], Color.white, 1.0f, 0.1f, 0, 0, true, true, true, Tweenmode.Quad_easeIn);
			s.addKeyframe (60, 125, 0, 0, point1, 1f, 1f, 2 * Mathf.PI, new Rotation[0], Color.white, 1.0f, 0.1f, 0, 1.0f, true, true, true, Tweenmode.Linear);
			s.addKeyframe (125, 130, 0, 0, point1, 1f, 1f, 2 * Mathf.PI, new Rotation[0], Color.white, 0, 0.1f, 0, 1.0f, true, true, true, Tweenmode.Linear);
			s.animOn ();
		}
		if (type == 3) {
			Vector3[] point1 = Frame.getRandomPoint (5, 4f, 3f);
			var inst = Instantiate (shape, transform.position, transform.rotation);

			Shape s = inst.GetComponent<Shape> ();
			s.addKeyframe (0, 60, 0, 0, point1, 1f, 1f, 0, new Rotation[0], Color.white, 1.0f, 0.1f, 0, 0, false, true, false, Tweenmode.Quad_easeIn);
			s.addKeyframe (60, 125, 0, 0, point1, 1f, 1f, 2 * Mathf.PI, new Rotation[0], Color.white, 1.0f, 0.1f, 0, 1.0f, false, true, false, Tweenmode.Linear);
			s.addKeyframe (125, 130, 0, 0, point1, 1f, 1f, 2 * Mathf.PI, new Rotation[0], Color.white, 0, 0.1f, 0, 1.0f, false, true, false, Tweenmode.Linear);
			s.animOn ();
		}
		if (type == 4) {
			Vector3[] point1 = Frame.getPolygonPoint (3 ,1f);
			var inst = Instantiate (shape, transform.position, transform.rotation);

			Shape s = inst.GetComponent<Shape> ();
			s.addKeyframe (0, 60, 0, 0, point1, 1f, 1f, 0, new Rotation[0], Color.white, 1.0f, 1f, 0, 1.0f, true, false, true, Tweenmode.Quad_easeIn);
			s.addKeyframe (60, 125, 0, 0, point1, 1f, 2f, 2 * Mathf.PI, new Rotation[0], Color.white, 1.0f, 1f, 0, 1.0f, true, false, true, Tweenmode.Linear);
			s.addKeyframe (125, 130, 0, 0, point1, 1f, 2f, 2 * Mathf.PI, new Rotation[0], Color.white, 0, 1f, 0, 1.0f, true, false, true, Tweenmode.Linear);
			s.animOn ();
		}
		if (type == 5) {
			Vector3[] point1 = Frame.getPolygonPoint (4 ,1f);
			var inst = Instantiate (shape, transform.position, transform.rotation);

			Shape s = inst.GetComponent<Shape> ();
			s.addKeyframe (0, 60, -20, -20, point1, 1f, 1f, 0, new Rotation[0], Color.white, 1f, 0.1f, 0, 1.0f, true, false, true, Tweenmode.Quad_easeIn);
			s.addKeyframe (60, 80, 20, 20, point1, 1f, 2f, 2 * Mathf.PI, new Rotation[0], Color.white, 0, 0.1f, 0, 1.0f, true, false, true, Tweenmode.Linear);
			s.addKeyframe (80, 85, 20, 20, point1, 1f, 2f, 2 * Mathf.PI, new Rotation[0], Color.white, 0, 0.1f, 0, 1.0f, true, false, true, Tweenmode.Linear);
			s.animOn ();
		}
		if (type == 6) {
			Vector3[] point1 = Frame.getPolygonPoint (4 ,1f);
			Rotation[] rota1 = new Rotation[] { new Rotation (0, 0, 0, 0) };
			Rotation[] rota2 = new Rotation[] { new Rotation (0, 0, 0, Mathf.PI * 2) };
			var inst = Instantiate (shape, transform.position, transform.rotation);

			Shape s = inst.GetComponent<Shape> ();
			s.addKeyframe (0, 60, 0, 0, point1, 1f, 1f, 0, rota1, Color.white, 1f, 0.1f, 0, 1.0f, true, false, true, Tweenmode.Quad_easeIn);
			s.addKeyframe (60, 125, 0, 0, point1, 1f, 2f, 2 * Mathf.PI, rota2, Color.white, 1.0f, 0.1f, 0, 1.0f, true, false, true, Tweenmode.Linear);
			s.addKeyframe (125, 130, 0, 0, point1, 1f, 2f, 2 * Mathf.PI, rota2, Color.white, 0, 0.1f, 0, 1.0f, true, false, true, Tweenmode.Linear);
			s.animOn ();
		}
		if (type == 7) {
			Vector3[] point1 = Frame.getPolygonPoint (3 ,1f);
			Rotation[] rota1 = new Rotation[] { new Rotation (-20, 0, 0, 0) };
			Rotation[] rota2 = new Rotation[] { new Rotation (-20, 0, 0, Mathf.PI * 2) };
			var inst = Instantiate (shape, transform.position, transform.rotation);

			Shape s = inst.GetComponent<Shape> ();
			s.addKeyframe (0, 60, 20, 0, point1, 1f, 1f, 0, rota1, Color.white, 1f, 0.1f, 0, 1.0f, true, false, true, Tweenmode.Quad_easeInOut);
			s.addKeyframe (60, 125, 20, 0, point1, 1f, 2f, 2 * Mathf.PI, rota2, Color.white, 1.0f, 0.1f, 0, 1.0f, true, false, true, Tweenmode.Linear);
			s.addKeyframe (125, 130, 20, 0, point1, 1f, 2f, 2 * Mathf.PI, rota2, Color.white, 0, 0.1f, 0, 1.0f, true, false, true, Tweenmode.Linear);
			s.animOn ();
		}
		if (type == 8) {
			//环形转圈扩散
			Vector3[] point1 = Frame.getRandomPoint (5, 4f, 3f);
			Vector3 pos = new Vector3 (midx, 0, 0);
			var inst = Instantiate (shape, pos, transform.rotation);
            Color col = Color.grey;
            Shape s = inst.GetComponent<Shape> ();

			s.addKeyframe (0, 60, 0, 0, point1, 1f, 1f,   0, new Rotation[0], col, 1.0f, 0.1f, 0, 1.0f, false, true, false, Tweenmode.Linear);
			s.addKeyframe (60, 70, 0, 0, point1, 10f, 1f, 2 * Mathf.PI, new Rotation[0], col, 0, 0.1f, 0, 1.0f, false, true, false, Tweenmode.Linear);
			s.animOn ();
		}
		if (type == 9) {
			//随机圆

			Vector3[] point1 = Frame.getRandomPoint (5, 4f, 3f);
			int i=0;
			Shape[] s = new Shape[5];
			for(int x=-2;x<3;x++)
				for(float y=-1.5f;y<2;y++)
			 {
				int posXrand = Random.Range(0,2);
				int posYrand = Random.Range(-1,1);
				int timerand = Random.Range (0, 3);
				Vector3 pos = new Vector3 (x*4+posXrand, y*3+posYrand, 0);
				var inst = Instantiate (shape, pos, transform.rotation);
                    Color col = Color.white;
                    s[i]= inst.GetComponent<Shape> ();
					s[i].addKeyframe (0, timerand, 0, 0, point1, 0, 1f, 2 * Mathf.PI, new Rotation[0], col, 1.0f, 0.1f, 0, 1.0f, false, false, true, Tweenmode.Linear);
					s[i].addKeyframe (timerand, timerand+30, 0, 0, point1, 0, 1f, 2 * Mathf.PI, new Rotation[0], col, 1.0f, 0.1f, 0, 1.0f, false, false, true, Tweenmode.Back_easeOut);
					s[i].addKeyframe (timerand+30, timerand+50, 0, 0, point1, 3f, 1f, 2 * Mathf.PI, new Rotation[0], col, 1.0f, 0.1f, 0, 1.0f, false, false, true, Tweenmode.Linear);
					s[i].addKeyframe (timerand+50, timerand+80, 0, 0, point1, 3f, 1f, 2 * Mathf.PI, new Rotation[0], col, 1.0f, 0.1f, 0, 1.0f, false, false, true, Tweenmode.Back_easeIn);
					s[i].addKeyframe (timerand+80, timerand+85, 0, 0, point1, 0, 1f, 2 * Mathf.PI, new Rotation[0], col, 1.0f, 0.1f, 0, 1.0f, false, false, true, Tweenmode.Linear);
				s[i].animOn ();
				i++;
			}
		}
		if (type == 10) {
			//线条横扫

			float amountrand = Random.Range (1, 5);
			float widthrand=Random.Range(0.5f,5f/ amountrand-0.5f);
			//
			float lengthrand = Random.Range (4, 8);
            Color col = Color.red;
            for (int i = 0; i < amountrand; i++) {
				Vector3[] point1 = new Vector3[2];
				point1[0]=new Vector3(-lengthrand/2,0,0);
				point1[1]=new Vector3(lengthrand/2,0,0);
				float posX =  midx;
				float posY = (i-(amountrand-1)/2)*10/amountrand;
				Vector3 pos = new Vector3 (posX, posY, 0);
				var inst = Instantiate (shape, pos+transform.localPosition, transform.rotation,transform);

				Shape s = inst.GetComponent<Shape> ();
				s.addKeyframe (0, 30, 0, 0, point1, 1f, 1f, 2 * Mathf.PI, new Rotation[0], col, 0.5f, widthrand, 0, 0, true, true, false, Tweenmode.Expo_easeOut);
				s.addKeyframe (30, 60, 0, 0, point1, 1f, 1f, 2 * Mathf.PI, new Rotation[0], col,0.5f, widthrand, 0, 1.0f, true, true, false, Tweenmode.Expo_easeOut);
				s.addKeyframe (60, 90, 0, 0, point1, 1f, 1f, 2 * Mathf.PI, new Rotation[0], col, 0, widthrand, 1.0f, 1.0f, true, true, false, Tweenmode.Linear);
				s.animOn ();
			}
		}
		if (type == 11) {
			//随机方
			int i=0;
			Shape[] s = new Shape[20];
			for(int x=-2;x<3;x++)
				for(float y=-1.5f;y<2;y++)
				{

					//int posXrand = Random.Range(-90,120);
					//int posYrand = Random.Range(-50,50);
					int posXrand = Random.Range(-20,20);
					int posYrand = Random.Range(-15,15);
					int timerand = Random.Range (0, 30);
					float sizerand = Random.Range (1, 3);
					Vector3[] point1 = Frame.getPolygonPoint (4 ,sizerand);
					//int temprand = Random.Range (6, 18);
					Vector3 pos = new Vector3 (x*40+posXrand, y*30+posYrand, 0);
					var inst = Instantiate (shape, pos, transform.rotation);
                    Color col = Color.black;
                    Debug.Log (col.ToString ());
					s[i]= inst.GetComponent<Shape> ();
					s[i].addKeyframe (0, timerand, 0, 0, point1, 0.1f, 1f, 2 * Mathf.PI, new Rotation[0], col, 1.0f, 0.1f, 0, 1.0f, true, false, true, Tweenmode.Linear);
					s[i].addKeyframe (timerand, timerand+30, 0, 0, point1, 0.1f, 1f, 2 * Mathf.PI, new Rotation[0], col, 1.0f, 0.1f, 0, 1.0f, true, false, true, Tweenmode.Back_easeOut);
					s[i].addKeyframe (timerand+30, timerand+50, 0, 0, point1, 3f, 1f, 2 * Mathf.PI, new Rotation[0], col, 1.0f, 0.1f, 0, 1.0f, true, false, true, Tweenmode.Linear);
					s[i].addKeyframe (timerand+50, timerand+80, 0, 0, point1, 3f, 1f, 2 * Mathf.PI, new Rotation[0], col, 1.0f, 0.1f, 0, 1.0f, true, false, true, Tweenmode.Back_easeIn);
					s[i].addKeyframe (timerand+80, timerand+85, 0, 0, point1, 0.1f, 1f, 2 * Mathf.PI, new Rotation[0], col, 1.0f, 0.1f, 0, 1.0f, true, false, true, Tweenmode.Linear);
					s[i].animOn ();
					i++;

				}
		}
		
		if (type == 13) {
			//按钮位置方形旋转淡入淡出
			Vector3[] point1 = Frame.getPolygonPoint (4 ,1f);

			var inst = Instantiate (shape, transform.position, transform.rotation);
			Rotation[] rota1 = new Rotation[] { new Rotation (0, 0, 0, Mathf.Atan(transform.position.y/transform.position.x)) };
			Rotation[] rota2 = new Rotation[] { new Rotation (0, 0, 0, Mathf.PI/8+Mathf.Atan(transform.position.y/transform.position.x)) };
			Rotation[] rota3 = new Rotation[] { new Rotation (0, 0, 0, Mathf.PI/4+Mathf.Atan(transform.position.y/transform.position.x)) };
			Shape s = inst.GetComponent<Shape> ();
			s.addKeyframe (0, 30, 0, 0, point1, 2f, 1f, 0, rota1, Color.white, 0, 0.1f, 0, 1.0f, true, false, true, Tweenmode.Expo_easeOut);
			s.addKeyframe (30, 60, 0, 0, point1, 2f, 2f, 2 * Mathf.PI, rota2, Color.white, 1f, 0.1f, 0, 1.0f, true, false, true, Tweenmode.Expo_easeIn);
			s.addKeyframe (60, 65, 0, 0, point1, 2f, 2f, 2 * Mathf.PI, rota3, Color.white, 0, 0.1f, 0, 1.0f, true, false, true, Tweenmode.Linear);
			s.animOn ();
		}
		if (type == 14) {
			//烟花
			float amountrand = Random.Range (3, 10);
			float widthrand=Random.Range(6f,-10/3*amountrand+30f);
			//
			float lengthrand = Random.Range (40, 120);
            Color col = Color.white;
            for (int i = 0; i < amountrand; i++) {
				Vector3[] point1 = new Vector3[2];
				point1 [0] = new Vector3 (-lengthrand / 2, 0, 0);
				point1 [1] = new Vector3 (lengthrand / 2, 0, 0);
				float posX = midx;
				float posY = (i - (amountrand - 1) / 2) * 100 / amountrand;
				Vector3 pos = new Vector3 (posX, posY, 0);
				var inst = Instantiate (shape, pos, transform.rotation);

				Shape s = inst.GetComponent<Shape> ();
				s.addKeyframe (0, 30, 0, 0, point1, 1f, 1f, 2 * Mathf.PI, new Rotation[0], col, 1.0f, widthrand, 0, 0, true, true, false, Tweenmode.Expo_easeOut);
				s.addKeyframe (30, 60, 0, 0, point1, 1f, 1f, 2 * Mathf.PI, new Rotation[0], col, 1.0f, widthrand, 0, 1.0f, true, true, false, Tweenmode.Expo_easeOut);
				s.addKeyframe (60, 90, 0, 0, point1, 1f, 1f, 2 * Mathf.PI, new Rotation[0], col, 0, widthrand, 1.0f, 1.0f, true, true, false, Tweenmode.Linear);
				s.animOn ();
			}
		}
		if (type == 15) {
			//修正随机圆
			int i=0;
			Shape[] s = new Shape[20];
			for(int x=-2;x<3;x++)
				for(float y=-1.5f;y<2;y++)
				{

					//int posXrand = Random.Range(-90,120);
					//int posYrand = Random.Range(-50,50);
					int posXrand = Random.Range(-20,20);
					int posYrand = Random.Range(-15,15);
					int timerand = Random.Range (0, 30);
					float sizerand = Random.Range (1, 2);
					Vector3[] point1 = Frame.getPolygonPoint (12 ,sizerand);
					//int temprand = Random.Range (6, 18);
					Vector3 pos = new Vector3 (x*40+posXrand, y*30+posYrand, 0);
					var inst = Instantiate (shape, pos, transform.rotation);
                    Color col = Color.white;
                    Debug.Log (col.ToString ());
					s[i]= inst.GetComponent<Shape> ();
					s[i].addKeyframe (0, timerand, 0, 0, point1, 0.1f, 1f, 2 * Mathf.PI, new Rotation[0], col, 1.0f, 0.1f, 0, 1.0f, true, false, true, Tweenmode.Linear);
					s[i].addKeyframe (timerand, timerand+30, 0, 0, point1, 0.1f, 1f, 2 * Mathf.PI, new Rotation[0], col, 1.0f, 0.1f, 0, 1.0f, true, false, true, Tweenmode.Back_easeOut);
					s[i].addKeyframe (timerand+30, timerand+50, 0, 0, point1, 3f, 1f, 2 * Mathf.PI, new Rotation[0], col, 1.0f, 0.1f, 0, 1.0f, true, false, true, Tweenmode.Linear);
					s[i].addKeyframe (timerand+50, timerand+80, 0, 0, point1, 3f, 1f, 2 * Mathf.PI, new Rotation[0], col, 1.0f, 0.1f, 0, 1.0f, true, false, true, Tweenmode.Back_easeIn);
					s[i].addKeyframe (timerand+80, timerand+85, 0, 0, point1, 0.1f, 1f, 2 * Mathf.PI, new Rotation[0], col, 1.0f, 0.1f, 0, 1.0f, true, false, true, Tweenmode.Linear);
					s[i].animOn ();
					i++;

				}
		}


	}
	public void SetType(int curtype){
		type = curtype;
	}

}
