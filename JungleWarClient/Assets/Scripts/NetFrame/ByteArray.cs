using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ByteArray
{
        private byte[] buffer = null; // 缓存区
        private int offset = 0;
        private int size = 0;

        /// <summary>
        /// 构造方法指定缓存的大小
        /// </summary>
        /// <param name="size"></param>
        public ByteArray(int size)
        {
            buffer = new byte[size];
            this.size = size;
            offset = 0;
        }

        /// <summary>
        /// 弹出前count位字节
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public byte[] PopRange(int count)
        {
            if (offset < count) return null;
            byte[] result = new byte[count];
            Buffer.BlockCopy(buffer, 0, result, 0, count); // 获取前几个字节
            Buffer.BlockCopy(buffer, count, buffer, 0, buffer.Length - count); // 将前几位元素从buffer中删除
            offset -= count;
            return result;
        }

        /// <summary>
        /// 获取前count位字节
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public byte[] PeekRange(int count)
        {
            if(offset < count) return null;
            byte[] result = new byte[count];
            Buffer.BlockCopy(buffer, 0, result, 0, count);
            return result;
        }

        /// <summary>
        /// 在尾部添加数据
        /// </summary>
        /// <param name="data"></param>
        public void AddLast(byte[] data)
        {
            if(offset + data.Length > size)
            { // 内存不足 重新分配空间
                byte[] temp = buffer;
                buffer = new byte[size += data.Length];
                Buffer.BlockCopy(temp, 0, buffer, 0, temp.Length); // 将旧数据复制到新数组
            }
            // 在尾部添加数据
            Buffer.BlockCopy(data, 0, buffer, offset, data.Length);
            offset += data.Length;
        }

        public int GetOffset()
        {
            return offset;
        }

        public void TestPrint()
        {
            Debug.Log("测试输出如下:----------------------------------");
            Debug.Log("offset:" + offset + " size:" + size + "\n");
            Debug.Log("buffer内数据如下");
            foreach (byte item in buffer)
                Debug.Log(item + " ");
            Debug.Log("测试输出如上:----------------------------------");
        }
}
