using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class PathManager : MonoBehaviour
    {
        [SerializeField] string path;

        public string Path
        {
            get { return CorrectPath(); }
        }

        protected string CorrectPath()
        {
            return System.IO.Path.Combine(Application.streamingAssetsPath, path);
        }
}