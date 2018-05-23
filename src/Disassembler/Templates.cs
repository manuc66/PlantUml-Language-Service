namespace PlantUmlLanguageService.Disassembler
{
    /// <summary>
    /// 
    /// </summary>
    public static class Templates
    {

        /// <summary>
        /// Gets or sets the psuedo code.
        /// </summary>
        /// <value>
        /// The psuedo code.
        /// </value>
        public static string PsuedoCode { get; set; } = "@startuml\n\n{0}\n\n@enduml";

        /// <summary>
        /// Gets or sets the main template.
        /// </summary>
        /// <value>
        /// The main template.
        /// </value>
        public static string MainTemplate { get; set; } = "{0}\n\n&#9974; [$PseudoNamespace]\n\n&larr; [$NavigationUp]\n\n&larr; [Back to Index]({3}/index.md)\n\n---\n\n[$Topography]\n\n---\n\n##Contents\n{1}\n\n---\n\n{2}\n";

        /// <summary>
        /// Gets or sets the class template.
        /// </summary>
        /// <value>
        /// The class template.
        /// </value>
        public static string ClassTemplate { get; set; } = @"
##{0}

&#9974; [$PseudoCode]

{2}

{1}

---

&uarr; [Back to Top]({3})

---

";

        /// <summary>
        /// Gets or sets the diagram template.
        /// </summary>
        /// <value>
        /// The diagram template.
        /// </value>
        public static string DiagramTemplate { get; set; } = @"
![{0}]({1} '{0}')";

	/// <summary>
	/// Gets or sets the function template.
	/// </summary>
	/// <value>
	/// The function template.
	/// </value>
	public static string FunctionTemplate { get; set; } = @"
 ####{0}

 - Summary: {3}

 + Inputs:  **{1}**

 + Returns: **{2}**

";

    /// <summary>
    /// Gets or sets the void template.
    /// </summary>
    /// <value>
    /// The void template.
    /// </value>
    public static string VoidTemplate { get; set; } = @"
 ####{0}

 - Summary: {2}

 + Inputs:  **{1}**

";

    /// <summary>
    /// Gets or sets the property template.
    /// </summary>
    /// <value>
    /// The property template.
    /// </value>
    public static string PropertyTemplate { get; set; } = @"
 ####{0}

 - Summary: {2}

 + Returns:  **{1}**

";

    /// <summary>
    /// Gets or sets the descriptor.
    /// </summary>
    /// <value>
    /// The descriptor.
    /// </value>
    public static string Descriptor { get; set; } = @"
[$prototype][$type][$name][$stereotype]{
[$body]
}
";

    /// <summary>
    /// Gets or sets the namespace descriptor.
    /// </summary>
    /// <value>
    /// The namespace descriptor.
    /// </value>
    public static string NamespaceDescriptor { get; set; } = "namespace [$name] {\n[$body]\n}";

    /// <summary>
    /// Gets or sets the class descriptor.
    /// </summary>
    /// <value>
    /// The class descriptor.
    /// </value>
    public static string ClassDescriptor { get; set; } = "[$prototype][$type][$name][$stereotype]";

}

}
