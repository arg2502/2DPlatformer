using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformerTest
{
    class ItemStack
    {
        // attribute to hold stack
        List<Item> stack = new List<Item>();

        // add a phrase to the stack
        public void Push(Item item)
        {
            stack.Add(item);
        }

        // gets top value of stack and removes it
        public Item Pop(int type)
        {
            // if stack is empty
            if (stack.Count == 0) return null;

            // if data is present
            Item itemRemoved = stack[stack.Count - 1];
            stack.RemoveAt(stack.Count - 1);
            itemRemoved.Type = type;
            return itemRemoved;
        }
    }
}
