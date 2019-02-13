using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour {
	[SerializeField] private Material material;
	private MeshCollider collider;
	private Frame frame;
	private Mesh mesh;
	private List<Frame> keyframe = new List<Frame> ();

	private bool linear;
	private bool outline;
	private bool connect;

	private bool isAnimOn = false;
	private int curStep = 0;
	private int curKeyframe = 0;
	// Use this for initialization
	void Start () {
		initMesh ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (isAnimOn) {
			updateKeyframe ();
		}
		if (isAnimOn) {
			updateShape ();
		}
	}

	void initMesh (){
		gameObject.AddComponent<MeshFilter> ();
		gameObject.AddComponent<MeshRenderer> ();
		gameObject.AddComponent<MeshCollider> ();
		gameObject.GetComponent<MeshRenderer> ().material = material;
		//gameObject.GetComponent<MeshCollider> ().convex = true;
		//gameObject.GetComponent<MeshCollider> ().isTrigger = true;
		mesh = GetComponent<MeshFilter>().mesh;
		mesh.Clear();  
	}

	void updateShape (){
		// update mesh
		mesh.Clear();  
		// get current frame status
		Frame curFrame = getCurrentFrame ();
		curFrame.apply();
		mesh.vertices = curFrame.getVertices ();
		mesh.triangles = curFrame.getTriangles ();

		gameObject.GetComponent<MeshCollider> ().sharedMesh = mesh;

		gameObject.GetComponent<MeshRenderer> ().material.color = curFrame.getColor ();

	}

	void updateKeyframe (){
		Frame thisFrame = keyframe [curKeyframe];
		if (curStep >= thisFrame.getTime ().end) {
			curKeyframe ++;
		}
		curStep++;
		//anim is finished
		if (curKeyframe >= keyframe.Count - 1) {
			animOff ();
			Destroy (gameObject);
			return;
		}
	}

	Frame getCurrentFrame (){
		if (curKeyframe >= keyframe.Count - 1) {
			return keyframe [keyframe.Count - 1];
		}
		Frame f1 = keyframe [curKeyframe];
		Frame f2 = keyframe [curKeyframe + 1];
		Frame f = new Frame(f1);
		Timestamp t = f.getTime();
		f.setPivot(Tween.value((float)(curStep-t.start),f1.getPivot(),f2.getPivot()-f1.getPivot(),(float)(t.end-t.start),f.getTweenmode()));
		for (int i = 0; i < f.getPoint().Length; i++) {
			f.setPoint(Tween.value((float)(curStep-t.start),f1.getPoint(i),f2.getPoint(i)-f1.getPoint(i),(float)(t.end-t.start),f.getTweenmode()),i);
		}
		for (int i = 0; i < f.getRotation().Length; i++) {
			f.setRotationAngle(Tween.value((float)(curStep-t.start),f1.getRotationAngle(i),f2.getRotationAngle(i)-f1.getRotationAngle(i),(float)(t.end-t.start),f.getTweenmode()),i);
		}
		f.setSize(Tween.value((float)(curStep-t.start),f1.getSize(),f2.getSize()-f1.getSize(),(float)(t.end-t.start),f.getTweenmode()));
		f.setArc(Tween.value((float)(curStep-t.start),f1.getArc(),f2.getArc()-f1.getArc(),(float)(t.end-t.start),f.getTweenmode()));
		f.setAlpha(Tween.value((float)(curStep-t.start),f1.getAlpha(),f2.getAlpha()-f1.getAlpha(),(float)(t.end-t.start),f.getTweenmode()));
		f.setProgress(Tween.value((float)(curStep-t.start),f1.getProgress(),f2.getProgress()-f1.getProgress(),(float)(t.end-t.start),f.getTweenmode()));

		return f;
	}

	public void addKeyframe (int _time_start, int _time_end, float _pivot_x, float _pivot_y, Vector3[] _point_list, float _size, float _radius, float _arc, Rotation[] _rotation_list, Color _color, float _alpha, float _weight, float _progress_start, float _progress_end, bool _is_linear, bool _is_outline, bool _is_connect, Tweenmode _tweenmode){
		Frame newFrame = new Frame (_time_start, _time_end, _pivot_x, _pivot_y,  _point_list, _size, _radius, _arc, _rotation_list, _color, _alpha, _weight, _progress_start, _progress_end, _is_linear, _is_outline, _is_connect, _tweenmode);
		keyframe.Add (newFrame);
	}
	public void addKeyframe (){
		Frame newFrame = new Frame ();
		keyframe.Add (newFrame);
	}

	public void animOn (){
		isAnimOn = true;
	}

	public void animOff (){
		isAnimOn = false;
	}

	public static Vector3[] getPolygonPoint (int edge, float radius){
		float angle = 2f * Mathf.PI / (float)edge;
		Vector3[] point = new Vector3[edge];
		for (int i = 0; i < edge; i++) {
			point [i] = new Vector3 (radius*Mathf.Cos(angle*(float)(i)),radius*Mathf.Sin(angle*(float)(i)),0);
		}
		return point;
	}

}
