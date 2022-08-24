#include <vector>
#include <iostream>
#include <map>
#include <algorithm>
using namespace std;
namespace Tahsin
{
    template <typename T>
    class HuffmanNode
    {
    public:
        T data;
        int count;
        HuffmanNode<T> *parent;
        HuffmanNode<T> *left;
        HuffmanNode<T> *right;
        HuffmanNode() {}
        HuffmanNode(T data, int count)
        {
            this->data = data;
            this->count = count;
        }
        HuffmanNode<T> *UniteWith(HuffmanNode<T> *node)
        {
            HuffmanNode<T> *n = new HuffmanNode(nullptr, node->count + count);
            if (count > node->count)
            {
                n->left = node;
                n->right = this;
            }
            else
            {
                n->left = node;
                n->right = this;
            }
            node->parent = parent = n;
            return n;
        }
        bool operator<(const HuffmanNode<T> *node) const
        {
            return count < node->count;
        }
        bool operator>(const HuffmanNode<T> *node) const
        {
            return count > node->count;
        }
        static void Traverse(HuffmanNode<T> *node)
        {
            if (node == nullptr)
                return;
            cout << (node->data) << " " << node->count << endl;
            Traverse(node->right);
            Traverse(node->left);
        }
    };
    template <typename T>
    HuffmanNode<T> *HuffmanAsNode(vector<T> data)
    {
        map<T, int> dataQuantity;
        vector<T> types;
        for (T t : data)
        {
            if (dataQuantity.count(t))
            {
                dataQuantity[t]++;
            }
            else
            {
                dataQuantity.insert(t, 1);
            }
            if (!count(types.begin(), types.end(), t))
            {
                types.push_back(t);
            }
        }
        vector<int> counts;
        for (T t : types)
        {
            counts.push_back(dataQuantity[t]);
        }
        vector<HuffmanNode<T> *> nodes;
        for (int i = 0; i < counts.size(); i++)
        {
            nodes.push_back(new HuffmanNode(types[i], counts[i]));
        }
        while (nodes.size() > 1)
        {
            HuffmanNode<T> *node0 = nodes[nodes.size() - 1];
            HuffmanNode<T> *node1 = nodes[nodes.size() - 2];
            nodes.erase(nodes.end() - 1);
            nodes.erase(nodes.end() - 1);
            nodes.push_back(node1->UniteWith(node0));
            sort(nodes.begin(), nodes.end());
        }
        return nodes[0];
    }
}