import { Button, Card, CardBody, CardHeader, Navbar, NavbarBrand, NavbarContent, Avatar } from "@heroui/react";

export default function Home() {
  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100 dark:from-gray-900 dark:to-gray-800">
      {/* Navigation */}
      <Navbar className="bg-white/70 backdrop-blur-md">
        <NavbarBrand>
          <p className="font-bold text-inherit">Events Coordinator</p>
        </NavbarBrand>
        <NavbarContent justify="end">
          <Avatar
            isBordered
            size="sm"
            src="https://i.pravatar.cc/150?u=a042581f4e29026704d"
          />
        </NavbarContent>
      </Navbar>

      {/* Main Content */}
      <main className="container mx-auto px-4 py-8">
        {/* Hero Section */}
        <div className="text-center mb-12">
          <h1 className="text-4xl md:text-6xl font-bold text-gray-900 dark:text-white mb-4">
            Welcome to Events Coordinator
          </h1>
          <p className="text-xl text-gray-600 dark:text-gray-300 mb-8 max-w-2xl mx-auto">
            A modern Next.js 15+ application powered by HeroUI components for seamless event management.
          </p>
          <div className="flex gap-4 justify-center">
            <Button color="primary" size="lg" variant="solid">
              Get Started
            </Button>
            <Button color="default" size="lg" variant="bordered">
              Learn More
            </Button>
          </div>
        </div>

        {/* Feature Cards */}
        <div className="grid md:grid-cols-3 gap-6">
          <Card className="py-4">
            <CardHeader className="pb-0 pt-2 px-4 flex-col items-start">
              <p className="text-tiny uppercase font-bold">Next.js 15+</p>
              <h4 className="font-bold text-large">Modern Framework</h4>
            </CardHeader>
            <CardBody className="overflow-visible py-2">
              <p className="text-small text-gray-600">
                Built with the latest Next.js features including App Router, Server Components, and more.
              </p>
            </CardBody>
          </Card>

          <Card className="py-4">
            <CardHeader className="pb-0 pt-2 px-4 flex-col items-start">
              <p className="text-tiny uppercase font-bold">HeroUI</p>
              <h4 className="font-bold text-large">Beautiful Components</h4>
            </CardHeader>
            <CardBody className="overflow-visible py-2">
              <p className="text-small text-gray-600">
                Modern, accessible, and customizable React components built on top of Tailwind CSS.
              </p>
            </CardBody>
          </Card>

          <Card className="py-4">
            <CardHeader className="pb-0 pt-2 px-4 flex-col items-start">
              <p className="text-tiny uppercase font-bold">TypeScript</p>
              <h4 className="font-bold text-large">Type Safety</h4>
            </CardHeader>
            <CardBody className="overflow-visible py-2">
              <p className="text-small text-gray-600">
                Full TypeScript support for better development experience and fewer runtime errors.
              </p>
            </CardBody>
          </Card>
        </div>

        {/* Demo Section */}
        <div className="mt-12 text-center">
          <h2 className="text-2xl font-bold text-gray-900 dark:text-white mb-6">
            HeroUI Components Demo
          </h2>
          <div className="flex flex-wrap gap-4 justify-center">
            <Button color="primary" variant="solid">Primary</Button>
            <Button color="secondary" variant="solid">Secondary</Button>
            <Button color="success" variant="bordered">Success</Button>
            <Button color="warning" variant="flat">Warning</Button>
            <Button color="danger" variant="ghost">Danger</Button>
          </div>
        </div>
      </main>
    </div>
  );
}
