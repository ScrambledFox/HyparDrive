using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AnimationPlayer : MonoBehaviour {

    public GameObject lightObject;

    [SerializeField]
    private List<AnimationObject> animationObjects = new List<AnimationObject>();
    float timer = 0;
    float timerBig = 0;
    int indexBgAnim =0;

    private int BackgroundAnims     = 3;
    private int Tech1SmallAnims     = 0;
    private int Tech2SmallAnims     = 0;
    private int Tech1BigAnims       = 0;
    private int Tech2BigAnims       = 0;
    private int Nature1SmallAnims   = 1;
    private int Nature2SmallAnims   = 0;
    private int Nature1BigAnims     = 0;
    private int Nature2BigAnims     = 0;
    private int TechCollabAnims     = 0;
    private int NatureCollabAnims   = 0;
    private int BigCollabAnims      = 0;

    public List<string> background = new List<string>();
    public List<string> tech1small = new List<string>();
    public List<string> tech1big = new List<string>();
    public List<string> tech2small = new List<string>();
    public List<string> tech2big = new List<string>();
    public List<string> nature1small = new List<string>();
    public List<string> nature1big = new List<string>();
    public List<string> nature2small = new List<string>();
    public List<string> nature2big = new List<string>();
    public List<string> techCollab = new List<string>();
    public List<string> natureCollab = new List<string>();
    public List<string> bigCollab = new List<string>();

    public void Awake()
    {
        // zoek voor naampje
        for (int i = 1; i <= BackgroundAnims; i++)
        {
            background.Add("Background(" + i + ")");
        }
        for (int i = 1; i <= Tech1SmallAnims; i++)
        {
            tech1small.Add("Tech1Small(" + i + ")");
        }
        for (int i = 1; i <= Tech2SmallAnims; i++)
        {
            tech2small.Add("Tech2Small(" + i + ")");
        }
        for (int i = 1; i <= Tech1BigAnims; i++)
        {
            tech1big.Add("Tech1Big(" + i + ")");
        }
        for (int i = 1; i <= Tech2BigAnims; i++)
        {
            tech2big.Add("Tech2Big(" + i + ")");
        }
        for (int i = 1; i <= Nature1SmallAnims; i++)
        {
            nature1small.Add("Nature1Small(" + i + ")");
        }
        for (int i = 1; i <= Nature2SmallAnims; i++)
        {
            nature2small.Add("Nature2Small(" + i + ")");
        }
        for (int i = 1; i <= Nature1BigAnims; i++)
        {
            nature1big.Add("Nature1Big(" + i + ")");
        }
        for (int i = 1; i <= Nature2BigAnims; i++)
        {
            nature2big.Add("Nature2Big(" + i + ")");
        }
        for (int i = 1; i <= TechCollabAnims; i++)
        {
            techCollab.Add("TechCollab(" + i + ")");
        }
        for (int i = 1; i <= NatureCollabAnims; i++)
        {
            natureCollab.Add("NatureCollab(" + i + ")");
        }
        for (int i = 1; i <= BigCollabAnims; i++)
        {
            bigCollab.Add("BigCollab(" + i + ")");
        }
    }

    public void Update()
    {
        //PlayAnimation elke 5min anim backgr
        timer -= Time.deltaTime;
        timerBig -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 10;
            Debug.Log("hoi");
            playBackgroundAnim(background[indexBgAnim]);
        }
        if (timerBig <= 0)
        {
            timerBig = 20;
            Debug.Log("Switch");
            indexBgAnim = Random.Range(0, background.Count);
        }



        for (int i = 0; i < animationObjects.Count; i++)
        {
            if (animationObjects[i].gameObject != null)
            {
                if (animationObjects[i].HasPreviousFrame((System.DateTime.Now.Ticks - animationObjects[i].animStartTicks) / 100000000f))
                {
                    KeyFrame referenceFrame = animationObjects[i].GetPreviousFrame((System.DateTime.Now.Ticks - animationObjects[i].animStartTicks) / 100000000f);
                    animationObjects[i].gameObject.transform.position = referenceFrame.position;
                    animationObjects[i].gameObject.transform.rotation = referenceFrame.rotation;
                    animationObjects[i].gameObject.transform.localScale = referenceFrame.scale;
                    animationObjects[i].gameObject.GetComponent<LightObject>().SetColor(referenceFrame.colour);
                }
                else
                {
                    // INVISILBE YAS
                    animationObjects[i].gameObject.GetComponent<LightObject>().SetColor(Color.magenta);
                }

                if ((System.DateTime.Now.Ticks - animationObjects[i].animStartTicks) / 100000000f >= 1)
                {
                    Debug.Log("deleeeeeete");
                    //system reboot.
                    animationObjects[i].DeleteMe();
                    Destroy(animationObjects[i].gameObject);
                }
            }
        }        
        animationObjects.RemoveAll(o => o.deleteMe == true);
    }

    public void playBackgroundAnim(string filename)
    {
        AnimationLoader.INSTANCE.LoadAnimation(filename);
    }

    public void HandleNewAnimation ( AnimationSaveData animation) {
        foreach (AnimationSaveData.Track track in animation.tracks) {
            Debug.Log("Instantiate new");
            GameObject go = Instantiate(lightObject);

            RegisterNewAnimationObject(go, track);
        }
    }

    private void RegisterNewAnimationObject ( GameObject go, AnimationSaveData.Track track ) {

        animationObjects.Add(new AnimationObject(go, track));

    }


    struct AnimationObject {

        public GameObject gameObject;
        public AnimationSaveData.Track track;
        public List<KeyFrame> keyFrames;
        public List<KeyFrame> frameBuffer;
        public long animStartTicks;
        public bool deleteMe;

        public AnimationObject ( GameObject gameObject, AnimationSaveData.Track track ) {
            this.gameObject = gameObject;
            this.track = track;
            this.frameBuffer = new List<KeyFrame>();
            this.keyFrames = new List<KeyFrame>();
            this.animStartTicks = System.DateTime.Now.Ticks;
            this.deleteMe = false;
            this.keyFrames = recastKeyframe(track.keyFrames);

            RecalculateBuffer();
        }

        public void DeleteMe()
        {
            deleteMe = true;
        }

        public List<KeyFrame> recastKeyframe(AnimationSaveData.KeyFrame[] oldKeyframes)
        {
            List<KeyFrame> keyframeNew = new List<KeyFrame>();
            foreach (AnimationSaveData.KeyFrame keyframe in oldKeyframes)
            {
                Debug.Log(keyframe.time);
                keyframeNew.Add(new KeyFrame(keyframe.time, keyframe.position, Quaternion.Euler(keyframe.rotation.x, keyframe.rotation.y, keyframe.rotation.z), keyframe.scale, keyframe.colour));
            }
            return keyframeNew;
        }


        public List<KeyFrame> GetKeyFrames()
        {
            return keyFrames;
        }
        public List<KeyFrame> GetKeyFrameBuffer()
        {
            return frameBuffer;
        }

        public bool HasPreviousFrame(float time)
        {
            return frameBuffer.FindLast(f => f.time <= time) != null ? true : false;
        }

        public KeyFrame GetPreviousFrame(float time)
        {
            return frameBuffer.FindLast(f => f.time <= time);
        }

        public KeyFrame GetLastKeyFrame(float time)
        {
            return keyFrames.FindLast(k => k.time <= time);
        }

        public bool HasLastKeyFrame(float time)
        {
            return keyFrames.FindLast(k => k.time <= time) == null ? false : true;
        }

        public void AddFrameToBuffer(KeyFrame frame)
        {
            this.frameBuffer.Add(frame);
            //Debug.Log("BUF" + frame.time + ", V:" + frame.position + ", R:{" + frame.position.x + ", " + frame.position.y + ", " + frame.position.z + "}");
        }

        public KeyFrame GetNextKeyFrame(float time)
        {
            return keyFrames.Find(k => k.time > time);
        }

        public bool HasNextKeyFrame(float time)
        {
            return keyFrames.Find(k => k.time > time) == null ? false : true;
        }

        public void RecalculateBuffer()
        {
            int KEYFRAME_START;
            int KEYFRAME_END;

            if (track.keyFrames.Length == 0)
            {
                // Do nothing, there are no keyframes.
                KEYFRAME_START = 0;
                KEYFRAME_END = -1;
            }
            else if (track.keyFrames.Length == 1)
            {
                KEYFRAME_START = Mathf.RoundToInt(track.keyFrames[0].time * AnimationCreatorManager.KEYFRAME_RATE);
                KEYFRAME_END = KEYFRAME_START;
            }
            else
            {
                KEYFRAME_START = Mathf.RoundToInt(track.keyFrames[0].time * AnimationCreatorManager.KEYFRAME_RATE);
                KEYFRAME_END = AnimationCreatorManager.KEYFRAME_RATE;
            }

#if UNITY_EDITOR
            Debug.Log("Added " + (KEYFRAME_END - KEYFRAME_START) + " keyframes.");
#endif


            for (int t = KEYFRAME_START; t <= KEYFRAME_END; t++)
            {
                if (frameBuffer.Count == 0)
                {
                    KeyFrame referenceFrame = GetLastKeyFrame((t + 0.50000f) / AnimationCreatorManager.KEYFRAME_RATE);
                    AddFrameToBuffer(new KeyFrame(referenceFrame.time, referenceFrame.position, referenceFrame.rotation, referenceFrame.scale, referenceFrame.colour));
                }

                if (HasNextKeyFrame((t + 0.5f) / AnimationCreatorManager.KEYFRAME_RATE))
                {
                    KeyFrame firstReferenceFrame = GetLastKeyFrame((t + 0.50000f) / AnimationCreatorManager.KEYFRAME_RATE);
                    KeyFrame lastReferenceFrame = GetNextKeyFrame((t + 0.50000f) / AnimationCreatorManager.KEYFRAME_RATE);

                    float timeConstant = (((float)t - firstReferenceFrame.time * AnimationCreatorManager.KEYFRAME_RATE) / (AnimationCreatorManager.KEYFRAME_RATE * (lastReferenceFrame.time - firstReferenceFrame.time)));
                    //Debug.Log("frame " + t + ", with tc " + timeConstant + ", lerped time " + Mathf.Lerp(firstReferenceFrame.time, lastReferenceFrame.time, timeConstant));

                    //Debug.Log(Vector3.Lerp(firstReferenceFrame.position, lastReferenceFrame.position, timeConstant));

                    AddFrameToBuffer(new KeyFrame(
                        Mathf.Lerp(firstReferenceFrame.time, lastReferenceFrame.time, timeConstant),
                        Vector3.Lerp(firstReferenceFrame.position, lastReferenceFrame.position, timeConstant),
                        Quaternion.Lerp(firstReferenceFrame.rotation, lastReferenceFrame.rotation, timeConstant),
                        Vector3.Lerp(firstReferenceFrame.scale, lastReferenceFrame.scale, timeConstant),
                        Color.Lerp(firstReferenceFrame.colour, lastReferenceFrame.colour, timeConstant))
                    );

                }
                else
                {
                    break;
                }
            }
        }
    }



}