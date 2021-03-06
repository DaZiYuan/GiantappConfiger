using GiantappConfiger.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Reflection;

namespace GiantappConfiger.Test
{
    [Descriptor(Text = "����")]
    class TestSetting
    {
        [Descriptor(Text = "����1")]
        public int P1 { get; set; }
        [Descriptor(Text = "����2", DefaultValue = "xxx")]
        public string P2 { get; set; }
        [Descriptor(Text = "����3")]
        public bool P3 { get; set; }
        [Descriptor(Text = "����4")]
        public string P4 { get; set; }
        [Descriptor(Text = "����5")]
        public SubSetting SubSetting { get; set; }
    }

    class SubSetting
    {
        [Descriptor(Text = "����6")]
        public string SP1 { get; set; }
        [Descriptor(Text = "����7", DefaultValue = "ooo")]
        public string SP2 { get; set; }
    }

    [TestClass]
    public class UnitTest
    {
        //[TestMethod]
        //public void TestGetVMFromAttribute()
        //{
        //    ConfigerService service = new ConfigerService();
        //    TestSetting setting = new TestSetting()
        //    {
        //        P1 = 1,
        //        P2 = null,
        //        P3 = true
        //    };

        //    var vm = service.GetVM(setting);
        //    Assert.IsTrue(vm.Nodes.Count == 1);
        //    Assert.IsTrue(vm.Nodes[0].Selected);
        //    Assert.IsTrue(vm.Nodes[0].Properties.Count == 4);
        //    Assert.IsTrue(vm.Nodes[0].Descriptor.Text == "node 0");
        //    var p1 = vm.Nodes[0].Properties[0];
        //    Assert.IsTrue(p1.Descriptor.Text == "int property");
        //    Assert.IsTrue((int)p1.Value == 1);
        //}

        [TestMethod]
        public void TestGetVM()
        {
            TestSetting setting = new TestSetting()
            {
                P1 = 1,
                P2 = null,
                P3 = true
            };

            var descriptor = new DescriptorInfoDict()
            {
                {"TestSetting",
                    new DescriptorInfo(){
                        Text = "node 0",
                        PropertyDescriptors = new DescriptorInfoDict() {
                            {"P1", new DescriptorInfo(){ Text="int property"} },
                            {"P2", new DescriptorInfo(){ Text="string property",DefaultValue="xxx"} },
                            {"SubSetting", new DescriptorInfo(){ Text="string property",DefaultValue=new SubSetting(),
                                PropertyDescriptors=new DescriptorInfoDict()
                                {
                                    {"SP1", new DescriptorInfo(){ Text="sub int property"} },
                                    {"SP2", new DescriptorInfo(){ Text="sub string property",DefaultValue="ooo"} }
                                }}
                            },
                        }
                    }}
            };
            var vm = ConfigerService.GetVM(setting, descriptor);
            Assert.IsTrue(vm.Nodes.Count == 1);
            Assert.IsTrue(vm.Nodes[0].Selected);
            Assert.IsTrue(vm.Nodes[0].Properties.Count == 4);
            Assert.IsTrue(vm.Nodes[0].Properties[1].Value.ToString() == "xxx");
            Assert.IsTrue(vm.Nodes[0].Descriptor.Text == "node 0");
            var p1 = vm.Nodes[0].Properties[0];
            Assert.IsTrue(p1.Descriptor.Text == "int property");
            Assert.IsTrue((int)p1.Value == 1);
            Assert.IsTrue(vm.Nodes[0].SubNodes[0].Properties[0].Descriptor.Text == "sub int property");
            Assert.IsTrue(vm.Nodes[0].SubNodes[0].Properties[1].Descriptor.Text == "sub string property");

            var tmpList = ConfigerService.GetAllData<TestSetting>(vm.Nodes);
            Assert.IsTrue(tmpList[0].P1 == 1);
            Assert.IsTrue(tmpList[0].P2 == "xxx");
            Assert.IsTrue(tmpList[0].P3);
            Assert.IsTrue(tmpList[0].SubSetting.SP2 == "ooo");
        }
    }
}
