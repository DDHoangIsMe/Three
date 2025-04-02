// using NUnit.Framework;
// using UnityEngine;

// public class DataManageTest
// {
//     private DataManage dataManage;
//     private GameObject mockBlock;
//     private RaycastHit mockHit;

//     [SetUp]
//     public void SetUp()
//     {
//         // Initialize DataManage instance
//         dataManage = new GameObject("DataManage").AddComponent<DataManage>();

//         // Mock TargetBlock GameObject
//         mockBlock = new GameObject("MockBlock");
//         mockBlock.AddComponent<TargetBlock>();
//         mockBlock.GetComponent<TargetBlock>().data_ = new BlockData { usable = true };

//         // Mock RaycastHit
//         mockHit = new RaycastHit
//         {
//             collider = mockBlock.GetComponent<Collider>()
//         };
//     }

//     [Test]
//     public void ActionBlock_TriggerUsableBlock()
//     {
//         // Arrange
//         mockBlock.GetComponent<TargetBlock>().data_.SetUsable(true);

//         // Act
//         dataManage.ActionBlock(mockHit);

//         // Assert
//         // Verify that the block was triggered (you can add specific checks if the trigger logic is implemented)
//         Assert.IsTrue(mockBlock.GetComponent<TargetBlock>().data_.IsUsable());
//     }

//     [Test]
//     public void ActionBlock_PlaceBlockWhenSpaceIsValid()
//     {
//         // Arrange
//         mockBlock.GetComponent<TargetBlock>().data_.SetUsable(false);
//         dataManage.gameObject.AddComponent<BoxCollider>(); // Mock box collider for CheckSpace

//         // Act
//         dataManage.ActionBlock(mockHit);

//         // Assert
//         // Verify that a new block was placed (you can add specific checks if PlaceBlock logic is implemented)
//         Assert.IsNotNull(mockBlock.GetComponent<TargetBlock>());
//     }

//     [Test]
//     public void ActionBlock_DoNothingWhenSpaceIsInvalid()
//     {
//         // Arrange
//         mockBlock.GetComponent<TargetBlock>().data_.SetUsable(false);
//         // Simulate invalid space by overriding CheckSpace to return false
//         var boxCollider = dataManage.gameObject.AddComponent<BoxCollider>();
//         boxCollider.enabled = false;

//         // Act
//         dataManage.ActionBlock(mockHit);

//         // Assert
//         // Verify that no block was placed
//         Assert.IsFalse(mockBlock.GetComponent<TargetBlock>().data_.IsUsable());
//     }
// }