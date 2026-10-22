
using System;
using UnityEngine;
using UnnamedUiToolkit;

[Serializable]
[UnnamedUiToolkit]
[DisallowMultipleComponent]
internal class InsoTestL0 : MonoBehaviour// : InsoComponentBase
{
    // [OnChange(nameof(CallbackTest))]
    // [field: SerializeField]
    // public int CallbackSource { get; set; }

    // [field: SerializeField]
    // public int CallbackCounter = 0;

    // private void CallbackTest() => CallbackCounter++;

    // [Button]
    // [DisplayName("Reset")]
    // public void ResetCounter() => CallbackCounter = 0;

    //private InsoTestL1 InsoTestL1Field;

    //public InsoTestL1 InsoTestL1PropertyNonSerialized { get; set; }

    //[field: SerializeField]
    //public InsoTestL1 InsoTestL1PropertySerialized { get; set; }

    public TestData InsoConfigTestNotSerialized { get; set; }

    [field: SerializeField]
    public TestData InsoConfigTestSerialized { get; set; }

    [DefaultSerializer]
    [field: SerializeField]
    public TestData InsoConfigTestSerializedDefault { get; set; }
    public bool FieldTest;

    // [Skip]
    // public bool SkipTest { get; set; }

    // [DefaultSerializer]
    // [field: SerializeField]
    // public int[] IntTest { get; set; }

    // [field: SerializeField]
    // public int[] IntTest2 { get; set; }
}

[Serializable]
internal class TestData
{
    // public bool FieldTest;

    // [Skip]
    // public bool SkipTest { get; set; }

    // [DefaultSerializer]
    // [field: SerializeField]
    // public int[] IntTest { get; set; }

    // [field: SerializeField]
    // public int[] IntTest2 { get; set; }

    public HumanBodyBones Bone = HumanBodyBones.LastBone;

    [OnChange(nameof(CallbackTest))]
    [field: SerializeField]
    public int CallbackSource { get; set; }

    [field: SerializeField]
    public int CallbackCounter = 0;

    private void CallbackTest() => CallbackCounter++;

    [Button]
    [DisplayName("Reset")]
    public void ResetCounter() => CallbackCounter = 0;

}

[Serializable]
internal class InsoTestL1 : TestData
{

    [field: SerializeField]
    public InsoTestL2 InsoTestL2 { get; set; }
}

[Serializable]
internal class InsoTestL2 : TestData
{
    [field: SerializeField]
    public InsoTestL3 InsoTestL3 { get; set; }
}

[Serializable]
internal class InsoTestL3 : TestData
{
    public string StringField;

    [field: SerializeField]
    public string StringProperty { get; set; }

    [field: SerializeField]
    public InsoConfigTest InsoConfigTest { get; set; }
}

[Serializable]
internal class InsoConfigTest
{
    public bool Bypass;

    [field: SerializeField]
    public AnimatorConfigTest AnimatorConfig { get; set; }

    [field: SerializeField]
    public PluginConfigTest PluginConfig { get; set; }

    [Button]
    public void SetTrue() => SetAll(true);

    [Button]
    public void SetFalse() => SetAll(false);

    private void SetAll(bool value)
    {
        AnimatorConfig.DeleteUnreachableStates = value;
        AnimatorConfig.ClearEmptyLayersAgressive = value;
        AnimatorConfig.ForceNoLoopOneFrameAnimation = value;
        AnimatorConfig.DeleteUselessBindings = value;
        AnimatorConfig.DeduplicateEmptyAnimations = value;
        AnimatorConfig.DeleteCombinerBypassStates = value;
        PluginConfig.DetailedReport = value;
        PluginConfig.AditionalConsoleLogging = value;
    }
}


[Serializable]
[DisallowMultipleComponent]
internal class AnimatorConfigTest
{
    [field: SerializeField]
    public bool DeleteUnreachableStates { get; set; }
    [field: SerializeField]
    public bool ClearEmptyLayersAgressive { get; set; }
    [field: SerializeField]
    public bool ForceNoLoopOneFrameAnimation { get; set; }
    [field: SerializeField]
    public bool DeleteUselessBindings { get; set; }
    [field: SerializeField]
    public bool DeduplicateEmptyAnimations { get; set; }
    [field: SerializeField]
    public bool DeleteCombinerBypassStates { get; set; }
}


[Serializable]
[DisallowMultipleComponent]
internal class PluginConfigTest
{
    [field: SerializeField]
    public bool DetailedReport { get; set; }
    [field: SerializeField]
    public bool AditionalConsoleLogging { get; set; }
}