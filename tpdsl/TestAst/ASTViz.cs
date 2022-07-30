/***
 * Excerpted from "Language Implementation Patterns",
 * published by The Pragmatic Bookshelf.
 * Copyrights apply to this code. It may not be used to create training material, 
 * courses, books, articles, and the like. Contact us if you are in doubt.
 * We make no guarantees that this code is fit for any purpose. 
 * Visit http://www.pragmaticprogrammer.com/titles/tpdsl for more book information.
***/
using Antlr4.StringTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAst
{
    public class ASTViz
    {
        private TemplateGroup _templates;
        private int _counter = 1; // used to make unique names
        private Tree _root;

        public ASTViz(Tree root)
        {
            _root = root;

            string directory = Directory.GetCurrentDirectory();
            string path = Path.Combine(directory, "DOT.stg");
            _templates = new TemplateGroupFile(path);
        }

        public override string ToString()
        {
            Template fileST = _templates.GetInstanceOf("file");
            fileST.Add("gname", "testgraph");
            _counter = 1;
            Walk(_root, fileST);
            return fileST.Render();
        }

        /// <summary>
        /// Fill fileST with nodes and edges; return subtree root's ST
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="fileST"></param>
        /// <returns></returns>
        protected Template Walk(Tree tree, Template fileST)
        {
            Template parentST = getNodeST(tree);
            fileST.Add("nodes", parentST); // define subtree root

            if (tree.GetChildCount() == 0) return parentST;

            // for each child, create nodes/edges and inject into fileST
            foreach (Tree child in tree.Children)
            {
                Template childST = Walk(child, fileST);
                object from = parentST.GetAttribute("name");
                object to = childST.GetAttribute("name");
                Template edgeST = getEdgeST(from, to);
                fileST.Add("edges", edgeST);
            }

            return parentST;
        }

        protected Template getEdgeST(Object from, Object to)
        {
            Template edgeST = _templates.GetInstanceOf("edge");
            edgeST.Add("from", from);
            edgeST.Add("to", to);
            return edgeST;
        }

        protected Template getNodeST(Tree t)
        {
            Template nodeST = _templates.GetInstanceOf("node");
            string uniqueName = "node" + _counter++; // use counter for unique name
            nodeST.Add("name", uniqueName);
            nodeST.Add("text", t.Payload);
            return nodeST;
        }
    }
}


