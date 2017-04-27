using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace _321_HW14 {
    public partial class Form1 : Form {

        Trie newTrie = new Trie();
        List<string> words = new List<string>();

        public Form1() {
            InitializeComponent();

            newTrie.loadFile();

            Thread newThread = new Thread(createTrie);
            newThread.Start();
            newThread.Join();
        }

        public void createTrie() {
            foreach (string x in newTrie.allWords) {
                newTrie.addString(x);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {

            if (string.IsNullOrWhiteSpace(textBox1.Text)) {
                newTrie.prefix = null;
                textBox2.Clear();
                return;
            }

            else {
                TrieNode n = newTrie.findParent(textBox1.Text);
                words = newTrie.returnWords(n);

                foreach (var word in words) {
                    textBox2.AppendText(word);
                    textBox2.AppendText(Environment.NewLine);
                }
            }
        }
    }

    public class TrieNode {
        public char C;
        public List<TrieNode> children = new List<TrieNode>();

        public TrieNode(char newChar) {
            C = newChar;
        }

        public TrieNode addChild(char cc) {
            foreach (TrieNode x in children) {
                if (x.C == cc) {
                    return x;
                }
            }
            
            var n = new TrieNode(cc);
            children.Add(n);
            return n;
        }
    }

    public class Trie {
        private TrieNode m_root = new TrieNode('\0');
        public List<string> allWords;
        public List<string> possibleWords = new List<string>();
        public string prefix = "";

        public void addString (string s) {
            TrieNode n = m_root;

            foreach (char sc in s) {
                n = n.addChild(sc);
            }
        }

        public void loadFile() {
            var wordFile = File.ReadAllLines("WordsEn.txt");
            allWords = new List<string>(wordFile);
        }

        public TrieNode findParent(string textBoxString) {
            TrieNode n = m_root;
            int i = 0;

            while (i < textBoxString.Length) {
                foreach (TrieNode x in n.children) {
                    if (x.C == textBoxString[i]) {
                        prefix += x.C;
                        n = x;
                        i++;
                        break;
                    }

                    else {
                        continue;
                    }
                }
            }

            if (n == m_root) {
                return null;
            }

            return n;
        }

        public List<string> returnWords (TrieNode parent) {
            List<string> possibleWords = new List<string>();

            for (int i = 0; i < parent.children.Count; i++) {
                TrieNode n = parent.children[i];

                findWords(possibleWords, n.C + "" , n, prefix);
            }

            return possibleWords;
        }

        private void findWords(List<string> possibleWords, string word, TrieNode n, string prefix) {

            if (n.children.Count == 0) {
                possibleWords.Add(prefix + word);
            }

            for (int i = 0; i < n.children.Count; i++) {
                n = n.children[i];
                findWords(possibleWords, word + n.C, n, prefix);
            }

        }
    }

}
