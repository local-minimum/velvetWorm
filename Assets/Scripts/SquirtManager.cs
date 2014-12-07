using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SquirtParticle {
	public Vector3 Position;
	public Vector3 Velocity;
	public int Team;
	
	public SquirtParticle(Vector3 position, Vector3 velocity, int team) {
		Position = position;
		Velocity = velocity;
		Team = team;
	}
}

public class Squirt {
	public bool Alive = true;
	public List<SquirtParticle> Particles = new List<SquirtParticle>();
	public Squirt NextSquirt = null;
	
	public void AddParticle(SquirtParticle p) {
		Particles.Add (p);  
	}
}

public class SquirtManager : MonoBehaviour {

	public LayerMask layers;
	public Material material;
	public Material ends;
	
	public List<Squirt> Squirts = new List<Squirt>();
	
	public float Width;
	private float halfWidth;
	public float Gravity;
	public float BreakingDistanceSquared = 4.0f;
	public ParticleSystem particleSystem;
	public GameObject self;
	public Camera mainCamera;
	private LevelCoordinator lvlCoord;
	private PlayerCoordinator playerCoord;

	void Start() {
		lvlCoord = GameObject.FindObjectOfType<LevelCoordinator>();
		playerCoord = gameObject.GetComponentInParent<PlayerCoordinator>();

		OnValidate();
	}
	
	void OnValidate() {
		halfWidth = Width / 2.0f;
	}
	
	public void AddSquirt(Squirt s) {
		Squirts.Add(s);
	}

	/*
	public Squirt AddParticle(Squirt lastSquirt, SquirtParticle p) {
		if(lastSquirt != null) {
			while(lastSquirt.NextSquirt != null)
				lastSquirt = lastSquirt.NextSquirt;
		}
		
		bool newSquirt = false;

		if(lastSquirt == null || !lastSquirt.Alive)
			newSquirt = true;
		else {
			int i = lastSquirt.Particles.Count;
			if(i > 0 && (lastSquirt.Particles[i - 1].Position - p.Position).sqrMagnitude > BreakingDistanceSquared)
				newSquirt = true;
			else
				newSquirt = false;
		}
		
		if(newSquirt) {
			// add to a new squrt
			Squirt newS = new Squirt();
			AddSquirt(newS);
			newS.AddParticle(p);
			return newS;
		} else {
			lastSquirt.AddParticle(p);
			return lastSquirt;
		}
	}
	*/

	void UpdateSquirt(float grav, float delta, Squirt s, ref Stack<Squirt> squirtsToAdd) {
		List<SquirtParticle> partsToDelete = null;
		int i = 0;
		SquirtParticle previous = null;
		bool breakChain = false;
		int maxPart = s.Particles.Count - 1;
		
		foreach(SquirtParticle p in s.Particles) {
			p.Velocity.y -= grav;
			Vector3 v = p.Velocity * delta;
			float mag = v.magnitude;
			v /= mag;
			
			bool die = false;
			
			RaycastHit2D[] hits = Physics2D.RaycastAll(p.Position, v, 1.5f * mag, layers);

			foreach(RaycastHit2D hit in hits) {
				if (hit.collider.gameObject == self)
					continue;

				die = true;

				Vector3 newVelocity = Vector3.Reflect(v, hit.normal);
				
				//particleSystem.Emit(p.Position, newVelocity * 3.0f, 0.3f, 0.2f, Color.white);
//				Debug.Log(hit.collider.gameObject.tag);
				if(hit.collider.gameObject.tag == "Player") {
					//Todo: hitcode

				} else if (hit.collider.gameObject.tag == "Enemy") {
					lvlCoord.RegisterKill(p.Team, hit.collider.gameObject);

				}
			}
			//}

			
			if(die) {
				//particleSystem.Emit(p.Position, p.Velocity * -0.2f, 0.3f, 0.2f, Color.white);
				
				// delete this particle
				if(partsToDelete == null)
					partsToDelete = new List<SquirtParticle>();
				
				partsToDelete.Add(p);
				previous = null;
				
				if(i != 0 && i != maxPart) {
					// start at next
					i++;
					breakChain = true;
					break;
				}
			} else {          
				p.Position += v;
			}

			/*
			if(previous != null) {
				float dist = (p.Position - previous.Position).sqrMagnitude;
				if(dist >= BreakingDistanceSquared) {
					breakChain = true;
					break;
				}
			}*/
			
			i++;
			previous = p;
		}
		
		if(breakChain) {
			Squirt newS = new Squirt();
			
			newS.Particles = s.Particles.GetRange(i, maxPart + 1 - i);
			s.Particles.RemoveRange(i, maxPart + 1 - i);
			if(squirtsToAdd == null)
				squirtsToAdd = new Stack<Squirt>();
			
			squirtsToAdd.Push (newS);
			if(s.NextSquirt == null)
				s.NextSquirt = newS;
		}
		
		if(partsToDelete != null) {
			// there particles to delete
			foreach(SquirtParticle p in partsToDelete)
				s.Particles.Remove(p);
			
			if(s.Particles.Count == 0) {
				s.Alive = false;
			}
		}
	}
	
	void FixedUpdate() {
		// do physics here
		float delta = Time.fixedDeltaTime;
		float grav = Gravity * delta;
		
		List<Squirt> squirtsToDelete = null;
		Stack<Squirt> squirtsToAdd = null;
		
		foreach(Squirt s in Squirts) {
			UpdateSquirt (grav, delta, s, ref squirtsToAdd);
			if(!s.Alive) {
				// delete this squirt
				if(squirtsToDelete == null)
					squirtsToDelete = new List<Squirt>();
				
				squirtsToDelete.Add(s);
			}
		}
		
		if(squirtsToAdd != null) {
			// a squirt broke, add its additional parts
			while(squirtsToAdd.Count > 0) {
				Squirt s = squirtsToAdd.Pop();
				UpdateSquirt(grav, delta, s, ref squirtsToAdd);
				if(s.Alive) {
					Squirts.Add(s);
				}
			}          
		}
		
		if(squirtsToDelete != null) {
			// there are squirts to delete
			foreach(Squirt s in squirtsToDelete)
				Squirts.Remove(s);
		}
	}
	
	void OnRenderObject() {

		Vector3 cameraPlaneX = mainCamera.transform.right;
		Vector3 cameraPlaneY = mainCamera.transform.up;
		Vector3 cameraDir = mainCamera.transform.forward;
		
		Vector3 x = new Vector3(cameraPlaneX.x * halfWidth,
		                        cameraPlaneX.y * halfWidth,
		                        cameraPlaneX.z * halfWidth);
		Vector3 y = new Vector3(cameraPlaneY.x * halfWidth,
		                        cameraPlaneY.y * halfWidth,
		                        cameraPlaneY.z * halfWidth);
		Vector3 z = cameraDir * halfWidth;
		
		material.SetPass(0);  
		GL.PushMatrix();
		//GL.MultMatrix(gameObject.transform.localToWorldMatrix);
		GL.SetRevertBackfacing(false);
		DrawEnds(ref x, ref y);
		DrawLines(ref x, ref y, ref mainCamera);
		GL.Color(Color.white);
		GL.PopMatrix();
	}
	
	void DrawEnds(ref Vector3 x, ref Vector3 y) {
		ends.SetPass(0);      
		
		GL.Begin(GL.TRIANGLES);
		
		foreach(Squirt s in Squirts) {
			foreach(SquirtParticle p in s.Particles) {
				// draw ends
				GL.TexCoord2(1, 0);
				GL.Vertex(p.Position + x - y);
				GL.TexCoord2(0, 0);
				GL.Vertex(p.Position - x - y);
				GL.TexCoord2(1, 1);
				GL.Vertex(p.Position + x + y);
				
				//GL.TexCoord2(1, 1);
				GL.Vertex(p.Position + x + y);
				GL.TexCoord2(0, 0);
				GL.Vertex(p.Position - x - y);
				GL.TexCoord2(0, 1);
				GL.Vertex(p.Position - x + y);
			}
		}
		GL.End ();
	}
	
	void DrawLines(ref Vector3 x, ref Vector3 y, ref Camera camera) {
		material.SetPass(0);
		GL.Begin(GL.TRIANGLES);
		
		foreach(Squirt s in Squirts) {
			SquirtParticle previous = null;
			Vector3 startCam = Vector3.zero;
			foreach(SquirtParticle p in s.Particles) {
				if(previous != null) {
					// draw line between previous and p
					Vector3 endCam = camera.WorldToScreenPoint(p.Position);
					startCam.z = 0;
					endCam.z = 0;
					
					Vector3 a, b, c, d;
					Vector3 n = Vector3.Cross(startCam, endCam);
					Vector3 l = Vector3.Cross(n, endCam-startCam);
					l.Normalize();
					l = new Vector3(x.x * l.x + y.x * l.y,
					                x.y * l.x + y.y * l.y,
					                x.z * l.x + y.z * l.y);
					
					a = (previous.Position + l);
					b = (previous.Position - l);
					c = (p.Position + l);
					d = (p.Position - l);
					
					// draw line
					GL.Vertex(a);
					GL.Vertex(b);
					GL.Vertex(c);
					
					GL.Vertex(c);
					GL.Vertex(b);
					GL.Vertex(d);
					
					startCam = endCam;
				} else {
					startCam = camera.WorldToScreenPoint(p.Position);
				}
				
				previous = p;
			}
		}
		GL.End ();
	}
}

