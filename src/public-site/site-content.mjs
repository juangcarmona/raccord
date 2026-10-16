const boundedContexts = [
    {
        id: "contacts",
        es: [
            "Contactos",
            "Clientes, proveedores y personas de contacto con los datos que el equipo necesita para trabajar y facturar.",
        ],
        en: [
            "Contacts",
            "Customers, suppliers and contact people with the details the team needs to work and bill.",
        ],
        features: {
            es: [
                "Organizaciones y personas",
                "Direcciones y datos fiscales",
                "Contactos asociados",
            ],
            en: [
                "Companies and people",
                "Addresses and tax details",
                "Related contacts",
            ],
        },
    },
    {
        id: "projects",
        es: [
            "Proyectos",
            "El trabajo audiovisual, sus fechas, tareas, notas y conceptos que después pueden facturarse.",
        ],
        en: [
            "Projects",
            "Audiovisual work, its dates, tasks, notes and the billable work it produces.",
        ],
        features: {
            es: [
                "Fechas y planificación",
                "Tareas y notas",
                "Trabajo facturable",
            ],
            en: ["Dates and planning", "Tasks and notes", "Billable work"],
        },
    },
    {
        id: "commercial",
        es: [
            "Comercial",
            "La parte económica de cada encargo: precios, ofertas, descuentos y aceptación del cliente.",
        ],
        en: [
            "Commercial",
            "The financial side of each job: pricing, offers, discounts and customer acceptance.",
        ],
        features: {
            es: [
                "Precios y descuentos",
                "Ofertas y versiones",
                "Aceptación del cliente",
            ],
            en: [
                "Pricing and discounts",
                "Offers and versions",
                "Customer acceptance",
            ],
        },
    },
    {
        id: "invoicing",
        es: [
            "Facturación",
            "Preparar, revisar y emitir facturas, y conservar el documento y el historial de una factura emitida.",
        ],
        en: [
            "Invoicing",
            "Prepare, review and issue invoices, then keep the issued document and its history.",
        ],
        features: {
            es: [
                "Borradores y revisión",
                "Numeración y emisión",
                "PDF e historial de auditoría",
            ],
            en: [
                "Drafts and review",
                "Numbering and issue",
                "PDF and audit history",
            ],
        },
    },
    {
        id: "equipment",
        es: [
            "Equipos",
            "El catálogo, los kits, las unidades y la disponibilidad del material que se alquila o se vende.",
        ],
        en: [
            "Equipment",
            "The catalogue, kits, individual units and availability of equipment for rental or sale.",
        ],
        features: {
            es: [
                "Catálogo y kits",
                "Unidades y accesorios",
                "Disponibilidad y reservas",
            ],
            en: [
                "Catalogue and kits",
                "Units and accessories",
                "Availability and reservations",
            ],
        },
    },
    {
        id: "warehouse",
        es: [
            "Almacén",
            "La preparación y el movimiento físico del material: listas, salidas, devoluciones e incidencias.",
        ],
        en: [
            "Warehouse",
            "Physical equipment handling: packing lists, dispatch, returns and exceptions.",
        ],
        features: {
            es: [
                "Listas de preparación",
                "Salidas y devoluciones",
                "Incidencias de material",
            ],
            en: [
                "Packing lists",
                "Dispatch and returns",
                "Equipment exceptions",
            ],
        },
    },
    {
        id: "crew",
        es: [
            "Equipo humano y transporte",
            "Personas, funciones, disponibilidad, turnos y vehículos relacionados con el trabajo.",
        ],
        en: [
            "Crew and transport",
            "People, roles, availability, shifts and vehicles connected to the work.",
        ],
        features: {
            es: [
                "Personas y funciones",
                "Disponibilidad y turnos",
                "Vehículos y transporte",
            ],
            en: [
                "People and roles",
                "Availability and shifts",
                "Vehicles and transport",
            ],
        },
    },
    {
        id: "documents",
        es: [
            "Documentos y comunicación",
            "Documentos de trabajo y comunicaciones que acompañan a operaciones, ofertas y facturas.",
        ],
        en: [
            "Documents and communication",
            "Work documents and communication that accompany operations, offers and invoices.",
        ],
        features: {
            es: [
                "Plantillas y documentos",
                "Archivos adjuntos",
                "Registro de comunicaciones",
            ],
            en: [
                "Templates and documents",
                "Attachments and files",
                "Communication history",
            ],
        },
    },
    {
        id: "workspace",
        es: [
            "Espacio de trabajo",
            "Usuarios, permisos, configuración de empresa y opciones para adaptar la instalación.",
        ],
        en: [
            "Workspace",
            "Users, permissions, company settings and options for configuring an installation.",
        ],
        features: {
            es: [
                "Usuarios y permisos",
                "Ajustes de empresa",
                "Configuración de instalación",
            ],
            en: [
                "Users and permissions",
                "Company settings",
                "Installation settings",
            ],
        },
    },
    {
        id: "compliance",
        es: [
            "Control y trazabilidad",
            "El registro de acciones y la información que acompaña a los documentos emitidos.",
        ],
        en: [
            "Compliance and audit",
            "A record of actions and the information associated with issued documents.",
        ],
        features: {
            es: [
                "Registro de acciones",
                "Trazabilidad de documentos",
                "Datos asociados a facturas",
            ],
            en: [
                "Action records",
                "Document traceability",
                "Invoice-linked records",
            ],
        },
    },
];

const content = {
    es: {
        language: "es",
        home: "Inicio",
        nav: {
            product: "Producto",
            manuals: "Manuales",
            hosting: "Autoalojamiento",
            engineering: "Arquitectura",
        },
        github: "Ver el proyecto",
        footer: "Software libre para las operaciones de producción audiovisual.",
        badge: "Para productoras audiovisuales y empresas de alquiler",
        heroTitle: "Tu producción y tus facturas, bajo tu control.",
        heroArtCaption: "Producción, de principio a fin",
        heroText:
            "Raccord reúne las operaciones de producción y la facturación en una aplicación que puedes alojar donde te convenga. Sin cuota de licencia: tú eliges y gestionas la infraestructura.",
        primary: "Explorar el producto",
        secondary: "Aprender a instalarlo",
        heroLabelProjects: "PROYECTOS",
        heroLabelInvoicing: "FACTURACIÓN",
        heroLabelHosting: "TU INFRAESTRUCTURA",
        promiseTitle:
            "El software es libre. La infraestructura es tu decisión.",
        promise:
            "Puedes preparar un servidor propio o desplegarlo en la nube. Raccord no cobra licencias; el servidor, el almacenamiento, las copias de seguridad y el mantenimiento pueden tener un coste.",
        heroNote:
            "El MVP está implementado. No ofrecemos una instancia alojada ni un registro público.",
        workflowTitle: "De un trabajo a su factura",
        workflowText:
            "El flujo completo ya se puede recorrer en la aplicación. Mira la captura y sigue la guía paso a paso.",
        steps: [
            "Registra el cliente y el proyecto",
            "Añade el trabajo facturable y revisa el borrador",
            "Emite la factura y descarga su PDF",
        ],
        screenshotTitle: "Una factura emitida, con su PDF y su historial.",
        screenshotCaption:
            "Captura real de una prueba automatizada con datos ficticios.",
        screenshotAlt:
            "Factura emitida de ejemplo con una línea de trabajo, total, descarga del PDF y registro de auditoría. Los datos son ficticios.",
        areasTitle: "Un producto pensado para el trabajo audiovisual",
        areasText:
            "El modelo de Raccord conecta diez áreas, desde la agenda del proyecto y los equipos hasta la factura y su trazabilidad.",
        areasNote:
            "Este mapa explica el alcance funcional del producto. La disponibilidad de cada función se está verificando por separado; la captura muestra el flujo de factura emitida con evidencia de navegador.",
        manualsTitle: "Guías para usarlo y mantenerlo",
        manualsText:
            "Instrucciones para tareas concretas, escritas para las personas que llevan producción, facturación y la instalación.",
        hostingTitle: "Elige dónde corre Raccord",
        hostingText:
            "Docker Compose es el camino de autoalojamiento documentado. Azure tiene una topología de despliegue propia. .NET Aspire se usa para desarrollo local, no para producción.",
        engineeringTitle: "Un sistema, con límites claros",
        engineeringText:
            "Conoce cómo se relacionan la interfaz, el servidor, la base de datos y las áreas del dominio, sin tener que empezar por el código.",
        availability: "Estado del producto",
        maturityTitle: "Qué esperar hoy",
        maturity:
            "El MVP está implementado y la web muestra un flujo de factura respaldado por una prueba de navegador y datos ficticios. El inventario detallado de las diez áreas está en revisión; consulta esta página como mapa del producto, no como promesa de que cada función está disponible. No hay demo alojada. Raccord no afirma cumplir requisitos legales o fiscales ni enviar facturas a organismos públicos.",
        productTitle: "Un mapa de las operaciones",
        productIntro:
            "Raccord conecta las áreas que suelen repartirse entre herramientas distintas. Este mapa resume qué responsabilidad cubre cada parte del modelo de producto.",
        productLabel: "Ámbito del modelo",
        manualTitle: "Manuales de Raccord",
        manualIntro:
            "Aprende una tarea concreta con instrucciones claras. Las guías actuales cubren el flujo de factura con evidencia de navegador y las rutas de instalación y operación.",
        manualInvoice: "Emitir y descargar una factura",
        manualInvoiceDesc:
            "Recorre el flujo que aparece en la captura pública: cliente, proyecto, concepto facturable, emisión y descarga del PDF.",
        manualInstall: "Instalar con Docker Compose",
        manualInstallDesc:
            "Prepara el archivo de configuración, inicia los servicios y crea la primera cuenta administradora en una red de confianza.",
        manualAzure: "Desplegar en Azure",
        manualAzureDesc:
            "Entiende los recursos de la plantilla y los pasos de despliegue antes de ejecutarlos en tu suscripción.",
        manualBackup: "Hacer una copia y restaurarla",
        manualBackupDesc:
            "Conserva base de datos y documentos juntos y practica la restauración antes de necesitarla.",
        prereq: "Antes de empezar",
        invoicePrereq:
            "Necesitas una instalación de Raccord, acceso con una cuenta autorizada y datos ficticios o de prueba. La guía describe la secuencia demostrada por la prueba de navegador; los nombres de los controles pueden cambiar entre versiones.",
        invoiceSteps: [
            "Entra en la instalación e inicia sesión.",
            "Registra un cliente de prueba.",
            "Crea un proyecto y añade una línea de trabajo facturable.",
            "Prepara el borrador y confirma sus datos antes de emitirlo.",
            "Abre la factura emitida y usa «Descargar PDF».",
            "Comprueba que el archivo descargado termina en .pdf y conserva el registro de la operación.",
        ],
        note: "La emisión de una factura puede tener consecuencias contables. Comprueba la configuración de tu empresa y consulta a tu asesoría antes de usar datos reales.",
        installSteps: [
            "Comprueba que tienes Docker Engine y Docker Compose en el servidor.",
            "Desde deploy/, copia .env.example a .env y configura la referencia de imagen y la contraseña de PostgreSQL.",
            "Mantén RACCORD_SEED_DEMO_DATA=false para una instalación normal.",
            "Ejecuta docker compose pull y docker compose up -d.",
            "Comprueba /health/ready y crea la primera cuenta administradora desde una red de confianza antes de abrir el acceso a otras personas.",
            "Configura TLS en el proxy inverso, no publiques el puerto de PostgreSQL y prepara copias cifradas fuera del servidor.",
        ],
        azureSteps: [
            "Revisa la plantilla Bicep, el grupo de recursos y la imagen de Raccord que vas a publicar.",
            "Configura el secreto como variable de entorno y limita su exposición; no lo escribas en un archivo ni en el repositorio.",
            "Despliega los recursos siguiendo la guía técnica vinculada.",
            "Inicia el trabajo de migración cuyo nombre devuelve el despliegue y comprueba el endpoint de disponibilidad.",
            "Antes de usarlo en producción, configura red privada, identidad, retención de copias y autenticación frente a la aplicación.",
        ],
        backupSteps: [
            "Con la instalación activa, ejecuta scripts/backup.sh desde deploy/.",
            "Confirma que la copia contiene el volcado de PostgreSQL, los documentos y el manifiesto de sumas SHA-256.",
            "Copia el conjunto a otro almacenamiento, cifrado y separado del servidor.",
            "En un entorno aislado, ejecuta scripts/restore.sh con la marca temporal de la copia.",
            "Comprueba la salud de la aplicación y que puedes consultar los documentos después de restaurar.",
        ],
        warningTitle: "Importante antes de continuar",
        installWarning:
            "La creación de la primera cuenta usa un endpoint disponible solo mientras no hay usuarios. Hazlo en una red de confianza y termina el alta antes de exponer la aplicación. No compartas la cuenta de demo ni publiques una instalación con datos de demostración.",
        azureWarning:
            "Esta guía describe el artefacto de despliegue, no autoriza ni ejecuta cambios en una suscripción. Revisa costes, exposición de red y secretos con quien administra tu entorno.",
        backupWarning:
            "Una copia que nunca se ha restaurado no demuestra que puedas recuperar el servicio. Repite el ensayo después de cambios importantes.",
        sourceDocs: "Referencias técnicas mantenidas",
        sourceSelfHost: "Guía completa de Docker Compose",
        sourceAzure: "Guía técnica de Azure",
        sourceBackup: "Guía de copia y restauración",
        architectureTitle: "Cómo está construido Raccord",
        architectureIntro:
            "La aplicación se ejecuta en tu entorno. El navegador habla con un servidor web; ese servidor guarda los datos en PostgreSQL y los documentos en un almacenamiento configurado.",
        diagramTitle: "Una instalación, tres piezas principales",
        diagramBrowser: "Navegador del equipo",
        diagramWeb: "Aplicación web y API",
        diagramDb: "PostgreSQL",
        diagramFiles: "Almacenamiento de documentos",
        diagramOps:
            "La persona que administra la instalación controla el alojamiento, el acceso, las actualizaciones y las copias de seguridad.",
        domainTitle: "El dominio sigue el trabajo",
        domainText:
            "Los límites de cada área ayudan a mantener junto lo que cambia por motivos parecidos: el proyecto produce trabajo facturable, facturación convierte ese trabajo en documentos emitidos, y la trazabilidad registra las acciones relacionadas.",
        domainLinks: "Ver las diez áreas del producto",
        devTitle: "Desarrollo y despliegue son caminos distintos",
        devText:
            ".NET Aspire sirve para iniciar dependencias y trabajar con Raccord durante el desarrollo. Para una instalación propia, sigue la guía de Docker Compose o la topología de Azure.",
        linkToRepo: "Consultar documentación técnica",
        back: "Volver a los manuales",
        page: {
            product: "Producto",
            manuals: "Manuales",
            hosting: "Autoalojamiento",
            engineering: "Arquitectura",
            invoice: "Emitir una factura",
            install: "Instalar con Docker Compose",
            azure: "Desplegar en Azure",
            backup: "Copias y restauración",
        },
        hostingTitlePage: "Aloja Raccord donde tenga sentido para tu equipo",
        hostingIntro:
            "No pagas una licencia de Raccord. Sí necesitas decidir quién mantiene el servidor, la base de datos, el almacenamiento, las copias y las actualizaciones.",
        hostOptions: [
            [
                "Tu propio servidor",
                "Controlas el entorno y las copias. Requiere mantener el sistema operativo, los contenedores, TLS y la recuperación.",
            ],
            [
                "Nube con Docker Compose",
                "Puedes usar una máquina virtual y seguir el mismo despliegue de Compose; el coste depende del proveedor y del tamaño del servidor.",
            ],
            [
                "Azure",
                "El repositorio incluye una plantilla Bicep con aplicación, base de datos, almacenamiento y recursos de soporte. Revisa el endurecimiento antes de producción.",
            ],
            [
                "Desarrollo local",
                ".NET Aspire facilita arrancar la aplicación y sus dependencias mientras contribuyes o pruebas cambios. No es la ruta de producción.",
            ],
        ],
        hostingCosts:
            "La licencia cuesta cero. La infraestructura, el almacenamiento, el correo externo si lo configuras, las copias fuera del servidor, el dominio y el tiempo de operación pueden generar costes.",
        hostingCta: "Abrir la guía paso a paso",
    },
    en: {
        language: "en",
        home: "Home",
        nav: {
            product: "Product",
            manuals: "Manuals",
            hosting: "Self-hosting",
            engineering: "Architecture",
        },
        github: "View the project",
        footer: "Open-source software for audiovisual production operations.",
        badge: "For audiovisual production and equipment-rental teams",
        heroTitle: "Production and invoicing, on your terms.",
        heroArtCaption: "Production, end to end",
        heroText:
            "Raccord brings production operations and invoicing together in an application you can host where it suits you. There is no licence fee; you choose and operate the infrastructure.",
        primary: "Explore the product",
        secondary: "Learn how to install it",
        heroLabelProjects: "PROJECTS",
        heroLabelInvoicing: "INVOICING",
        heroLabelHosting: "YOUR INFRASTRUCTURE",
        promiseTitle: "The software is free. Infrastructure is your choice.",
        promise:
            "Run it on your own server or deploy it in the cloud. Raccord has no licence fee; servers, storage, backups and maintenance may still cost money.",
        heroNote:
            "The MVP is implemented. There is no hosted instance or public sign-up.",
        workflowTitle: "From a job to its invoice",
        workflowText:
            "You can follow the full workflow in the application. See the screenshot and use the step-by-step guide.",
        steps: [
            "Register the customer and project",
            "Add billable work and review the draft",
            "Issue the invoice and download its PDF",
        ],
        screenshotTitle: "An issued invoice, with its PDF and history.",
        screenshotCaption:
            "Real screenshot from an automated test with fictional data.",
        screenshotAlt:
            "Example issued invoice with one billable line, a total, PDF download and audit history. All data is fictional.",
        areasTitle: "A product shaped around audiovisual work",
        areasText:
            "Raccord connects ten areas, from project planning and equipment to invoicing and traceability.",
        areasNote:
            "This map explains the product's functional scope. Availability of each capability is being checked separately; the screenshot shows the issued-invoice workflow with browser evidence.",
        manualsTitle: "Guides to use and run it",
        manualsText:
            "Task-based instructions for the people coordinating production, billing and the installation.",
        hostingTitle: "Choose where Raccord runs",
        hostingText:
            "Docker Compose is the documented self-hosting path. Azure has its own deployment topology. .NET Aspire is for local development, not production.",
        engineeringTitle: "One system, with clear boundaries",
        engineeringText:
            "See how the interface, server, database and domain areas fit together, without starting with the source code.",
        availability: "Product status",
        maturityTitle: "What to expect today",
        maturity:
            "The MVP is implemented, and this site shows an invoice workflow backed by browser-test evidence and fictional data. The detailed inventory across all ten areas is still under review; use this page as a product map, not a promise that every capability is available. There is no hosted demo. Raccord does not claim legal or tax compliance or submission of invoices to public authorities.",
        productTitle: "A map of the work",
        productIntro:
            "Raccord connects areas that are often split across different tools. This map summarises the responsibility of each part of the product model.",
        productLabel: "Product model scope",
        manualTitle: "Raccord manuals",
        manualIntro:
            "Learn a task with clear instructions. Current guides cover the invoice workflow with browser evidence and the installation and operations paths.",
        manualInvoice: "Issue and download an invoice",
        manualInvoiceDesc:
            "Follow the workflow shown in the public screenshot: customer, project, billable work, issue and PDF download.",
        manualInstall: "Install with Docker Compose",
        manualInstallDesc:
            "Prepare configuration, start the services and create the first administrator account on a trusted network.",
        manualAzure: "Deploy on Azure",
        manualAzureDesc:
            "Understand the template resources and deployment steps before running them in your subscription.",
        manualBackup: "Back up and restore",
        manualBackupDesc:
            "Keep the database and documents together, and practise restoring them before you need to.",
        prereq: "Before you start",
        invoicePrereq:
            "You need a Raccord installation, an authorised account and fictional or test data. This guide follows the sequence demonstrated by the browser test; control labels can change between versions.",
        invoiceSteps: [
            "Open your installation and sign in.",
            "Register a test customer.",
            "Create a project and add one billable work line.",
            "Prepare the draft and confirm its details before issuing it.",
            "Open the issued invoice and choose “Download PDF”.",
            "Check that the downloaded file ends in .pdf and keep the operation record.",
        ],
        note: "Issuing an invoice can have accounting consequences. Check your company setup and ask your adviser before using real data.",
        installSteps: [
            "Check that Docker Engine and Docker Compose are available on the server.",
            "From deploy/, copy .env.example to .env and configure the image reference and PostgreSQL password.",
            "Keep RACCORD_SEED_DEMO_DATA=false for a normal installation.",
            "Run docker compose pull and docker compose up -d.",
            "Check /health/ready and create the first administrator account on a trusted network before opening access to others.",
            "Configure TLS at the reverse proxy, do not publish PostgreSQL, and prepare encrypted backups off the server.",
        ],
        azureSteps: [
            "Review the Bicep template, resource group and Raccord image you plan to publish.",
            "Provide secrets through environment variables and limit their exposure; do not write them to a file or repository.",
            "Deploy the resources using the linked technical guide.",
            "Start the migration job returned by the deployment and check the readiness endpoint.",
            "Before production use, configure private networking, identity, backup retention and authentication in front of the application.",
        ],
        backupSteps: [
            "With the installation running, execute scripts/backup.sh from deploy/.",
            "Confirm the backup contains a PostgreSQL dump, documents and a SHA-256 checksum manifest.",
            "Copy the set to separate storage away from the server, with encryption.",
            "In an isolated environment, run scripts/restore.sh with the backup timestamp.",
            "Check application health and that documents are available after restore.",
        ],
        warningTitle: "Before you continue",
        installWarning:
            "Creating the first account uses an endpoint that is available only while no users exist. Do this on a trusted network and finish account setup before exposing the application. Never share the demo account or expose an installation with demo data.",
        azureWarning:
            "This guide describes the deployment artifact; it does not authorise or execute changes in a subscription. Review costs, network exposure and secrets with the person who operates your environment.",
        backupWarning:
            "A backup that has never been restored does not prove you can recover the service. Repeat the drill after major changes.",
        sourceDocs: "Maintained technical references",
        sourceSelfHost: "Full Docker Compose guide",
        sourceAzure: "Azure technical guide",
        sourceBackup: "Backup and restore guide",
        architectureTitle: "How Raccord is built",
        architectureIntro:
            "The application runs in your environment. A browser talks to a web server; that server stores data in PostgreSQL and documents in configured storage.",
        diagramTitle: "One installation, three main parts",
        diagramBrowser: "Team browser",
        diagramWeb: "Web application and API",
        diagramDb: "PostgreSQL",
        diagramFiles: "Document storage",
        diagramOps:
            "The person operating the installation controls hosting, access, updates and backups.",
        domainTitle: "The domain follows the work",
        domainText:
            "Boundaries keep together the things that change for similar reasons: a project produces billable work, invoicing turns that work into issued documents, and traceability records related actions.",
        domainLinks: "Explore the ten product areas",
        devTitle: "Development and deployment take different paths",
        devText:
            ".NET Aspire helps start dependencies and work on Raccord during development. For a self-hosted installation, follow the Docker Compose guide or Azure topology.",
        linkToRepo: "Read technical documentation",
        back: "Back to manuals",
        page: {
            product: "Product",
            manuals: "Manuals",
            hosting: "Self-hosting",
            engineering: "Architecture",
            invoice: "Issue an invoice",
            install: "Install with Docker Compose",
            azure: "Deploy on Azure",
            backup: "Backup and restore",
        },
        hostingTitlePage: "Host Raccord where it works for your team",
        hostingIntro:
            "There is no Raccord licence fee. You do need to decide who maintains the server, database, storage, backups and updates.",
        hostOptions: [
            [
                "Your own server",
                "You control the environment and backups. You are responsible for the operating system, containers, TLS and recovery.",
            ],
            [
                "Cloud with Docker Compose",
                "You can use a virtual machine and the same Compose deployment; cost depends on provider and server size.",
            ],
            [
                "Azure",
                "The repository includes a Bicep template for the application, database, storage and supporting resources. Review hardening before production.",
            ],
            [
                "Local development",
                ".NET Aspire helps start the application and dependencies while contributing or testing changes. This is not a production path.",
            ],
        ],
        hostingCosts:
            "The licence costs nothing. Infrastructure, storage, external email if configured, off-site backups, a domain and operating time may incur costs.",
        hostingCta: "Open the step-by-step guide",
    },
};

export function getSiteContent(locale) {
    return { ...content[locale], boundedContexts };
}
