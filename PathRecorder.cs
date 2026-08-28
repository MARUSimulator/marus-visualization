// Copyright 2022 Laboratory for Underwater Systems and Technologies (LABUST)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using UnityEngine;
using System.IO;
using Marus.Logger;

namespace Marus.Visualization
{
    /// <summary>
    /// This script records object position at the given rate.
    /// Positions are stored in a binary file in PathRecordings folder with timestamp in file name.
    /// </summary>
    public class PathRecorder : MonoBehaviour
    {
        /// <summary>
        /// Position sample rate in Hz.
        /// </summary>
        public float SampleRateHz = 5;

        [Tooltip("Prefix used for the saved recording files.")]
        public string FilePrefix = "PathRecording";

        private bool _enabled = true;

        /// <summary>
        /// Minimum distance in meters between points to be recorded.
        /// </summary>
        private float MinimumDistanceDelta = 0.1f;
        private float _timer = 0f;
        private Vector3 _lastPosition;
        private GameObjectLogger<Vector3> logger;
        private string topic;
        private string savePath;

        void Start()
        {
            savePath = Path.Combine(Application.dataPath, "PathRecordings");
            RefreshTopic();
            logger = DataLogger.Instance.GetLogger<Vector3>(topic);
        }

        private void RefreshTopic()
        {
            int index = GetNextFileIndex();
            topic = $"{FilePrefix}-{index}-";
        }

        private int GetNextFileIndex()
        {
            // Ensure directory exists to prevent GetFiles from throwing an exception
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            string[] fileEntries = Directory.GetFiles(savePath);
            int index = 0;

            foreach(string fileName in fileEntries)
            {
                var f = Path.GetFileName(fileName);
                if (f.EndsWith(".json") && f.StartsWith(FilePrefix))
                {
                    index++;
                }
            }

            return index + 1;
        }

        void Update()
        {
            if (!_enabled)
            {
                return;
            }

            var distanceDeltaCondition = Vector3.Distance(_lastPosition, transform.position) > MinimumDistanceDelta;
            if (_timer >= (1 / SampleRateHz) && distanceDeltaCondition)
            {
                logger.Log(transform.position);
                _lastPosition = transform.position;
                _timer = 0;
            }
            _timer += Time.deltaTime;
        }

        public bool IsEnabled()
        {
            return _enabled;
        }

        public void Enable()
        {
            _enabled = true;
        }

        public void Disable()
        {
            if (!_enabled) return; // Prevent double-saving if called multiple times

            RefreshTopic();
            _enabled = false;
            DataLoggerUtilities.SaveLogsForTopic(topic, savePath);
        }

        void OnDisable()
        {
            if (_enabled) // Only save on disable if it hasn't been saved manually yet
            {
                DataLoggerUtilities.SaveLogsForTopic(topic, savePath);
                _enabled = false;
            }
        }
    }
}