using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace CubeGenerateMod
{
    public class Program : MelonMod
    {
        private string _enterStrNumber = "";
        private Transform _playerTransform = new Transform();

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            _playerTransform = GameObject.Find("/Player/XR Origin").transform;
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                _enterStrNumber += "0";
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _enterStrNumber += "1";
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _enterStrNumber += "2";
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _enterStrNumber += "3";
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                _enterStrNumber += "4";
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                _enterStrNumber += "5";
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                _enterStrNumber += "6";
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                _enterStrNumber += "7";
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                _enterStrNumber += "8";
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                _enterStrNumber += "9";
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                GenerateObject();
            }
        }

        private void GenerateObject()
        {
            if (_enterStrNumber == "")
            {
                Melon<Program>.Logger.Msg("Enter number");
                return;
            }

            bool isDone = false;
            int generateType = -1;
            int objNumber = -1;
            try
            {
                string generateTypeStr = _enterStrNumber.Substring(0, 1);
                string objNumberStr = _enterStrNumber.Substring(1);

                generateType = int.Parse(generateTypeStr);
                objNumber = int.Parse(objNumberStr);

                if (1 < generateType)
                    throw new ArgumentOutOfRangeException();

                isDone = true;
            }
            catch (FormatException)
            {
                Melon<Program>.Logger.Msg("Invalid number: FormatException");
            }
            catch (ArgumentOutOfRangeException)
            {
                Melon<Program>.Logger.Msg("Invalid number: ArgumentOutOfRangeException");
            }

            if (!isDone)
            {
                _enterStrNumber = string.Empty;
                return;
            }
            switch (generateType)
            {
                case 0:
                    Melon<Program>.Logger.Msg($"Generate cube with substance {objNumber}");
                    CubeGenerator.GenerateCube(
                        _playerTransform.position + _playerTransform.forward * 2f + _playerTransform.up * 0.5f,
                        _playerTransform.rotation,
                        new Vector3(1, 1, 1),
                        (Substance)objNumber
                        );
                    break;
                case 1:
                    switch (objNumber)
                    {
                        case 0:
                            CubeGenerator.GenerateBattery(_playerTransform.position, _playerTransform.rotation, 1f);
                            break;
                        case 1:
                            CubeGenerator.GenerateBuoy(_playerTransform.position);
                            break;
                        default:
                            Melon<Program>.Logger.Msg("Invalid number");
                            break;
                    }
                    break;
                default:
                    Melon<Program>.Logger.Msg("Invalid number");
                    break;
            }
            _enterStrNumber = string.Empty;
        }
    }
}
