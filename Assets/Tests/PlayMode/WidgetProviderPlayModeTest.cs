using System.Collections;
using Code.Infrastructure.StateMachine;
using Code.Infrastructure.StateMachine.Game.States;
using Code.Services.Providers;
using Code.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Zenject;

namespace Tests.PlayMode
{
    public class WidgetProviderPlayModeTest
    {
        private IPoolProvider<Widget> _provider;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return LoadInitialScene();
            yield return null;
            
            var container = ProjectContext.Instance.Container;
            var sm = container.Resolve<IStateMachine<IGameState>>();
            yield return sm.Enter<LoadLevelState, string>("Game");
            
            _provider = container.Resolve<IPoolProvider<Widget>>();
            Assert.IsNotNull(_provider, "WidgetProvider should not be null");

            _provider.CreatePool();
            yield return null;
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            foreach (var obj in Object.FindObjectsOfType<GameObject>())
            {
                if (obj != null && !obj.scene.IsValid())
                    Object.Destroy(obj);
            }
            yield return null;
        }

        [UnityTest]
        public IEnumerator Should_Reuse_Play_Animation_Widget()
        {
            Widget first = _provider.Get(Vector3.zero, Quaternion.identity);
            Assert.IsNotNull(first, "First widget is null. Possibly the factory returned null.");

            first.SetText("Test");
            first.SetColor(Color.red);
            first.PlayAnimation();
            
            _provider.Return(first);
            yield return new WaitForSeconds(0.5f);
            
            Widget reused = _provider.Get(Vector3.right, Quaternion.identity);
            Assert.IsNotNull(reused, "Reused widget is null.");

            reused.SetText("Test_1");
            reused.SetColor(Color.gray);
            reused.PlayAnimation();
            
            Assert.AreSame(first, reused, "Expected the widget to be reused, but a different instance was returned.");
            yield return null;
        }

        private static IEnumerator LoadInitialScene()
        {
            var asyncLoad = SceneManager.LoadSceneAsync("Initial");
            yield return new WaitUntil(() => asyncLoad.isDone);
            yield return null;
        }
    }
}
